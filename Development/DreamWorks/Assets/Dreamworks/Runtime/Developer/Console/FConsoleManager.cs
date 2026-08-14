using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Log;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Commands;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Variables;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console
{
    public sealed class FConsoleManager
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly Dictionary<string, IConsoleCommand> commands;

        private readonly Dictionary<string, IConsoleVariable> variables;
        #endregion

        #region Properties
        public Action<string> OnCommandEntered { get; internal set; }

        public Action<FConsoleExecutionResult> OnCommandExecuted { get; internal set; }
        #endregion

        #region Constructors
        public FConsoleManager(ILogProvider logProvider)
        {
            this.logProvider = logProvider;

            commands = new Dictionary<string, IConsoleCommand>(StringComparer.OrdinalIgnoreCase);

            variables = new Dictionary<string, IConsoleVariable>(StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            RegisterAttributedCommands();
        }

        public void ShutDown()
        {
            commands.Clear();

            variables.Clear();
        }

        public void RegisterCommand(IConsoleCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            commands[command.Name] = command;
        }

        public void UnregisterCommand(string commandName)
        {
            commands.Remove(commandName);
        }

        public void RegisterVariable(IConsoleVariable variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            variables[variable.Name] = variable;
        }

        public void UnregisterVariable(string variableName)
        {
            variables.Remove(variableName);
        }

        public FConsoleVariable<T> RegisterVariable<T>(string name, T defaultValue, string description)
        {
            FConsoleVariable<T> variable = new(name, description, defaultValue);

            RegisterVariable(variable);

            return variable;
        }

        public bool TryGetVariable(string name, out IConsoleVariable variable)
        {
            return variables.TryGetValue(name, out variable);
        }

        public bool TryGetCommand(string name, out IConsoleCommand command)
        {
            return commands.TryGetValue(name, out command);
        }

        public IReadOnlyCollection<IConsoleCommand> GetCommands()
        {
            return commands.Values;
        }

        public IReadOnlyCollection<IConsoleVariable> GetVariables()
        {
            return variables.Values;
        }

        public FConsoleExecutionResult Execute(string commandLine)
        {
            OnCommandEntered?.Invoke(commandLine);

            string[] tokens = FConsoleTokenizer.Tokenize(commandLine);

            if (tokens.Length == 0)
            {
                return new(false, "Empty command.");
            }

            string objectName = tokens[0];

            if (commands.TryGetValue(objectName, out IConsoleCommand command))
            {
                FConsoleExecutionResult result = ExecuteCommand(command, tokens);

                OnCommandExecuted?.Invoke(result);

                return result;
            }

            if (variables.TryGetValue(objectName, out IConsoleVariable variable))
            {
                FConsoleExecutionResult result = ExecuteVariable(variable, tokens);

                OnCommandExecuted?.Invoke(result);

                return result;
            }

            FConsoleExecutionResult unknownResult = new(false, $"Unknown command '{objectName}'.");

            OnCommandExecuted?.Invoke(unknownResult);

            return unknownResult;
        }

        public IReadOnlyList<FConsoleSuggestion> GetSuggestions(string text)
        {
            List<FConsoleSuggestion> results = new();

            foreach (IConsoleCommand command in commands.Values)
            {
                if (command.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(new FConsoleSuggestion(command.Name, command.Description));
                }
            }

            foreach (IConsoleVariable variable in variables.Values)
            {
                if (variable.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(new FConsoleSuggestion(variable.Name, variable.Description));
                }
            }

            return results.OrderBy(x => x.Name).ToList();
        }
        #endregion

        #region Private Methods
        private FConsoleExecutionResult ExecuteCommand(IConsoleCommand command, string[] tokens)
        {
            try
            {
                string[] arguments = tokens.Skip(1).ToArray();

                object result = command.Execute(arguments);

                return new(true, $"Executed '{command.Name}: {result}'.");
            }
            catch (Exception exception)
            {
                logProvider.LogError(exception.ToString());

                return new(false, exception.Message);
            }
        }

        private FConsoleExecutionResult ExecuteVariable(IConsoleVariable variable, string[] tokens)
        {
            if (tokens.Length == 1)
            {
                return new(true, $"{variable.Name} = {variable.GetValue()}");
            }

            bool success = variable.TrySetValue(tokens[1]);

            if (!success)
            {
                return new(false, $"Invalid value for '{variable.Name}'.");
            }

            return new(true, $"{variable.Name} = {variable.GetValue()}");
        }

        private void RegisterAttributedCommands()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type[] types;

                try
                {
                    types = assembly.GetTypes();
                }
                catch
                {
                    continue;
                }

                foreach (Type type in types)
                {
                    MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                    foreach (MethodInfo method in methods)
                    {
                        AConsoleCommandAttribute attribute = method.GetCustomAttribute<AConsoleCommandAttribute>();

                        if (attribute == null)
                        {
                            continue;
                        }

                        FConsoleCommand command = new(attribute.Name, attribute.Description, method);

                        RegisterCommand(command);
                    }
                }
            }
        }
        #endregion
    }
}