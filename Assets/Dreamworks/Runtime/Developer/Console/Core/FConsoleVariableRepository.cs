using System;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;

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

        private readonly Dictionary<string, IConsoleVariable> registeredVariables;
        #endregion


        #region Constructors
        public FConsoleVariableRepository(ILogProvider logProvider)
        {
            this.logProvider = logProvider ?? FDefaultLogger.Instance;

            registeredVariables = new Dictionary<string, IConsoleVariable>(StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region IDeveloperConsoleVariableRepository Implementation
        void IConsoleVariableRepository.RegisterVariable(IConsoleVariable variable)
        {
            if (variable == null)
            {
                logProvider.LogError("Attempted to register a null variable.");

                return;
            }

            registeredVariables[variable.Name] = variable;

            logProvider.Log($"\"{variable.Name}\" variable has been registered.");
        }

        void IConsoleVariableRepository.RegisterVariable<TVariableType>(string name, TVariableType defaultValue, string description)
        {
            FConsoleVariable<TVariableType> variable = new(name, description, defaultValue);

            ((IConsoleVariableRepository)this).RegisterVariable(variable);
        }

        void IConsoleVariableRepository.UnregisterVariable(string variableName)
        {
            registeredVariables.Remove(variableName);

            logProvider.Log($"\"{variableName}\" variable has been unregistered.");
        }

        bool IConsoleVariableRepository.TryGetVariable(string name, out IConsoleVariable variable)
        {
            return registeredVariables.TryGetValue(name, out variable);
        }

        bool IConsoleVariableRepository.TryGetVariableValue<TVariableType>(string name, out TVariableType value)
        {
            value = default;

            if (registeredVariables.TryGetValue(name, out IConsoleVariable variable))
            {
                value = (TVariableType)variable.GetValue();

                return true;
            }

            return false;
        }
        #endregion

        #region IDeveloperConsoleInitialization Implementation
        void IDeveloperConsoleInitializer.Initialize()
        {
            
        }

        void IDeveloperConsoleInitializer.ShutDown()
        {

        }
        #endregion

        #region IDeveloperConsoleCommandQuery Implementation
        IReadOnlyList<FConsoleCommandSuggestion> IConsoleCommandQuery.QuerySuggestions(string text)
        {
            var results = new List<FConsoleCommandSuggestion>();

            foreach (var variable in registeredVariables.Values)
            {
                if (variable.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(new FConsoleCommandSuggestion(variable.Name, variable.Description));
                }
            }

            return results;
        }
        #endregion
    }
}