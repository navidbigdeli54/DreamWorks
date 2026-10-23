using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Definitions;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Persistence;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Core
{
    /// <summary>
    /// Provides a repository for managing console variables and their associated metadata.
    /// </summary>
    /// <remarks>This class implements <see cref="IConsoleVariableRepository"/>, <see
    /// cref="IDeveloperConsoleInitializer"/>,  and <see cref="IConsoleCommandQuery"/> to support the registration,
    /// retrieval, and management of console variables,  as well as initialization and shutdown of the developer
    /// console. It also provides functionality for querying  console command suggestions based on user input.</remarks>
    internal sealed class FConsoleVariableRepository : IConsoleVariableRepository, IDeveloperConsoleInitializer, IConsoleCommandQuery
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly IConsoleVariableDefinitionProvider definitionProvider;

        private readonly IConsoleVariablePersistence persistence;

        private readonly Dictionary<string, IConsoleVariable> registeredVariables = new Dictionary<string, IConsoleVariable>(StringComparer.OrdinalIgnoreCase);

        private readonly HashSet<IConsoleVariable> persistentSubscriptions = new HashSet<IConsoleVariable>();

        private bool isInitialized;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates the runtime repository with definition and persistence dependencies.
        /// </summary>
        public FConsoleVariableRepository(ILogProvider logProvider, IConsoleVariableDefinitionProvider definitionProvider, IConsoleVariablePersistence persistence)
        {
            this.logProvider = logProvider;

            this.definitionProvider = definitionProvider;

            this.persistence = persistence;
        }
        #endregion

        #region IConsoleVariableRepository Implementation
        void IConsoleVariableRepository.RegisterVariable(IConsoleVariable variable)
        {
            RegisterVariable(variable);
        }

        IConsoleVariable IConsoleVariableRepository.RegisterVariable<TVariableType>(string name, TVariableType defaultValue, string description, bool isPersistent)
        {
            return RegisterVariable(new FConsoleVariable<TVariableType>(name, description, defaultValue, isPersistent));
        }

        void IConsoleVariableRepository.UnregisterVariable(string variableName)
        {
            if (!registeredVariables.TryGetValue(variableName, out IConsoleVariable variable))
            {
                logProvider.LogVerbose($"Could not unregister console variable \"{variableName}\" because it is not registered.");

                return;
            }

            UnsubscribeFromVariable(variable);

            registeredVariables.Remove(variableName);

            logProvider.LogVerbose($"Unregistered console variable \"{variableName}\".");

            if (isInitialized)
            {
                SavePersistentValues();
            }
        }

        bool IConsoleVariableRepository.TryGetVariable(string name, out IConsoleVariable variable)
        {
            return registeredVariables.TryGetValue(name, out variable);
        }

        bool IConsoleVariableRepository.TryGetVariable<TVariableType>(string name, out FConsoleVariable<TVariableType> variable)
        {
            variable = null;

            if (registeredVariables.TryGetValue(name, out IConsoleVariable variableBase))
            {
                variable = variableBase as FConsoleVariable<TVariableType>;
            }

            return variable != null;
        }

        bool IConsoleVariableRepository.TryGetVariableValue<TVariableType>(string name, out TVariableType value)
        {
            value = default;

            if (!registeredVariables.TryGetValue(name, out IConsoleVariable variable) || variable.GetValue() is not TVariableType typedValue)
            {
                return false;
            }

            value = typedValue;

            return true;
        }

        IReadOnlyList<IConsoleVariable> IConsoleVariableRepository.GetRegisteredVariables() => registeredVariables.Values.ToArray();
        #endregion

        #region IDeveloperConsoleInitializer Implementation
        void IDeveloperConsoleInitializer.Initialize()
        {
            if (isInitialized)
            {
                logProvider.LogWarning("Console variable repository is already initialized; ignoring the repeated initialization.");

                return;
            }

            if (persistence == null)
            {
                logProvider.LogWarning("No console variable persistence is configured; persistent variables will not be restored or saved.");
            }

            persistence?.Load();

            RegisterDefinedVariables();

            SubscribeToPersistentVariables();

            isInitialized = true;

            logProvider.LogDisplay($"Console variable repository initialized with {registeredVariables.Count} variable(s), {persistentSubscriptions.Count} persistent.");
        }

        void IDeveloperConsoleInitializer.ShutDown()
        {
            if (!isInitialized)
            {
                return;
            }

            logProvider.LogVerbose($"Saving {persistentSubscriptions.Count} persistent console variable(s) on shutdown.");

            SavePersistentValues();

            foreach (IConsoleVariable variable in persistentSubscriptions)
            {
                variable.OnValueChanged -= OnPersistentVariableValueChanged;
            }

            persistentSubscriptions.Clear();

            isInitialized = false;
        }
        #endregion

        #region IConsoleCommandQuery Implementation
        IReadOnlyList<FConsoleCommandSuggestion> IConsoleCommandQuery.QuerySuggestions(string text)
        {
            List<FConsoleCommandSuggestion> results = new();
            foreach (IConsoleVariable variable in registeredVariables.Values)
            {
                if (variable.Name.StartsWith(text ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(new FConsoleCommandSuggestion(variable.Name, variable.Description));
                }
            }
            return results;
        }
        #endregion

        #region Private Methods
        private IConsoleVariable RegisterVariable(IConsoleVariable variable)
        {
            if (variable == null)
            {
                logProvider.LogWarning("Ignored a null console variable registration.");

                return null;
            }

            if (!FConsoleVariableDefinition.IsValidName(variable.Name))
            {
                logProvider.LogError($"Rejected console variable registration because \"{variable.Name}\" is not a valid variable name.");

                return null;
            }

            if (registeredVariables.TryGetValue(variable.Name, out IConsoleVariable existingVariable))
            {
                logProvider.LogWarning($"Console variable \"{variable.Name}\" is already registered and will be replaced.");

                UnsubscribeFromVariable(existingVariable);
            }

            if (persistence.TryGetValue(variable.Name, out string existedSavedValue))
            {
                if (variable.TrySetValue(existedSavedValue))
                {
                    logProvider.LogVerbose($"Restored persisted value \"{existedSavedValue}\" for console variable \"{variable.Name}\".");
                }
                else
                {
                    logProvider.LogWarning($"Persisted value \"{existedSavedValue}\" is not valid for console variable \"{variable.Name}\" of type {variable.ValueType.Name}; keeping its default value.");
                }
            }

            registeredVariables[variable.Name] = variable;

            if (isInitialized)
            {
                SubscribeToVariable(variable);
            }

            logProvider.LogVeryVerbose($"Registered console variable \"{variable.Name}\" ({variable.ValueType.Name}, persistent: {variable.IsPersistent}) = \"{variable.GetValue()}\".");

            return variable;
        }

        private void RegisterDefinedVariables()
        {
            IReadOnlyList<FConsoleVariableDefinition> definitions = definitionProvider?.Definitions;
            if (definitions == null)
            {
                logProvider.LogWarning("No console variable definition provider is available; skipping defined variables.");

                return;
            }

            HashSet<string> definitionNames = new(StringComparer.OrdinalIgnoreCase);
            for (int index = 0; index < definitions.Count; index++)
            {
                FConsoleVariableDefinition definition = definitions[index];
                if (definition == null)
                {
                    logProvider.LogWarning($"Console variable definition at index {index} is empty and was skipped.");

                    continue;
                }

                if (!definition.TryGetDefaultValue(out object defaultValue))
                {
                    logProvider.LogWarning($"Console variable definition \"{definition.Name}\" has an invalid default value for type {definition.VariableType} and was skipped.");

                    continue;
                }

                if (!definitionNames.Add(definition.Name))
                {
                    logProvider.LogWarning($"Duplicate console variable definition \"{definition.Name}\" at index {index} was skipped; the first definition wins.");

                    continue;
                }

                RegisterDefinition(definition, defaultValue);
            }

            logProvider.LogMessage($"Processed {definitions.Count} console variable definition(s); {definitionNames.Count} unique definition(s) submitted for registration.");
        }

        private void RegisterDefinition(FConsoleVariableDefinition definition, object defaultValue)
        {
            switch (definition.VariableType)
            {
                case EConsoleVariableType.Boolean: RegisterVariable(new FConsoleVariable<bool>(definition.Name, definition.Description, (bool)defaultValue, definition.IsPersistent)); break;
                case EConsoleVariableType.String: RegisterVariable(new FConsoleVariable<string>(definition.Name, definition.Description, (string)defaultValue, definition.IsPersistent)); break;
                case EConsoleVariableType.Float: RegisterVariable(new FConsoleVariable<float>(definition.Name, definition.Description, (float)defaultValue, definition.IsPersistent)); break;
                case EConsoleVariableType.Integer: RegisterVariable(new FConsoleVariable<int>(definition.Name, definition.Description, (int)defaultValue, definition.IsPersistent)); break;
                default: logProvider.LogError($"Console variable definition \"{definition.Name}\" uses unsupported type {definition.VariableType}."); break;
            }
        }

        private void SubscribeToPersistentVariables()
        {
            foreach (IConsoleVariable variable in registeredVariables.Values)
            {
                SubscribeToVariable(variable);
            }
        }

        private void SubscribeToVariable(IConsoleVariable variable)
        {
            if (persistence == null || !variable.IsPersistent || !persistentSubscriptions.Add(variable))
            {
                return;
            }

            variable.OnValueChanged += OnPersistentVariableValueChanged;
        }

        private void UnsubscribeFromVariable(IConsoleVariable variable)
        {
            if (!persistentSubscriptions.Remove(variable))
            {
                return;
            }

            variable.OnValueChanged -= OnPersistentVariableValueChanged;
        }

        private void OnPersistentVariableValueChanged(object value)
        {
            SavePersistentValues();
        }

        private void SavePersistentValues()
        {
            if (persistence == null)
            {
                return;
            }

            List<FConsoleVariablePersistenceRecord> records = new();

            foreach (IConsoleVariable variable in registeredVariables.Values)
            {
                if (variable.IsPersistent)
                {
                    records.Add(new FConsoleVariablePersistenceRecord(variable.Name, ToInvariantString(variable.GetValue())));
                }
            }

            persistence.Save(records);

            logProvider.LogVeryVerbose($"Saved {records.Count} persistent console variable value(s).");
        }

        private static string ToInvariantString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value is IFormattable formattable ? formattable.ToString(null, CultureInfo.InvariantCulture) : value.ToString();
        }
        #endregion
    }
}
