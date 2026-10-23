using System;
using System.Linq;
using System.Reflection;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Core
{
    /// <summary>
    /// Provides functionality to execute console commands and manage their execution results.
    /// </summary>
    /// <remarks>This class acts as an activator for console commands, allowing the execution of methods and
    /// variables registered in the console system. It raises events to notify when a command is entered and when its
    /// execution is completed. The class is designed to work with repositories for console methods and
    /// variables.</remarks>
    internal sealed class FConsoleCommandActivator : IConsoleCommandActivator
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly IConsoleMethodRepository methodRepository;

        private readonly IConsoleVariableRepository variableRepository;
        #endregion

        #region Events
        public event Action<string> OnCommandEntered;

        public event Action<FConsoleCommandExecutionResult> OnCommandExecuted;
        #endregion

        #region Constructors
        public FConsoleCommandActivator(ILogProvider logProvider, IConsoleMethodRepository methodRepository, IConsoleVariableRepository variableRepository)
        {
            this.logProvider = logProvider;
            this.methodRepository = methodRepository;
            this.variableRepository = variableRepository;
        }
        #endregion

        #region IDeveloperConsoleExecuter Implementation

        FConsoleCommandExecutionResult IConsoleCommandActivator.ExecuteCommand(string commandLine)
        {
            logProvider.LogVerbose($"Executing command line \"{commandLine}\".");

            OnCommandEntered?.Invoke(commandLine);

            string[] tokens = FConsoleCommandTokenizer.Tokenize(commandLine);

            if (tokens.Length == 0)
            {
                logProvider.LogWarning("Ignored an empty console command.");

                var emptyCommandResult = new FConsoleCommandExecutionResult(false, "Trying to execute empty command!");
                OnCommandExecuted?.Invoke(emptyCommandResult);
                return emptyCommandResult;
            }

            string commandName = tokens.First();

            if (methodRepository.TryGetMethod(commandName, out IConsoleMethod method))
            {
                FConsoleCommandExecutionResult result = ExecuteConsoleMethod(method, tokens.Skip(1).ToArray());

                return result;
            }

            if (variableRepository.TryGetVariable(commandName, out IConsoleVariable variable))
            {
                FConsoleCommandExecutionResult result = ExecuteConsoleVariable(variable, tokens.Skip(1).ToArray());

                return result;
            }

            logProvider.LogWarning($"Unknown console command \"{commandName}\". No method or variable is registered with this name.");

            var unknownResult = new FConsoleCommandExecutionResult(false, $"Unknown command \"{commandName}\".");

            OnCommandExecuted?.Invoke(unknownResult);

            return unknownResult;
        }
        #endregion

        #region Private Methods
        private FConsoleCommandExecutionResult ExecuteConsoleMethod(IConsoleMethod method, string[] arguments)
        {
            try
            {
                object result = method.Execute(arguments);

                logProvider.LogMessage($"Console method \"{method.Name}\" executed with {arguments.Length} argument(s). Result: {result ?? "<null>"}.");

                var executionResult = new FConsoleCommandExecutionResult(true, $"\"{method.Name}\" method has been executed. result: {result}.");

                OnCommandExecuted?.Invoke(executionResult);

                return executionResult;
            }
            catch (Exception exception)
            {
                // Reflection wraps the method's own exception; report the original cause.
                Exception cause = exception is TargetInvocationException { InnerException: not null } ? exception.InnerException : exception;

                logProvider.LogError(cause, $"Console method \"{method.Name}\" failed with arguments [{string.Join(", ", arguments)}].");

                var executionResult = new FConsoleCommandExecutionResult(false, $"An exception thrown when executing \"{method}\" method. exception: {exception}");

                OnCommandExecuted?.Invoke(executionResult);

                return executionResult;
            }
        }

        private FConsoleCommandExecutionResult ExecuteConsoleVariable(IConsoleVariable variable, string[] arguments)
        {
            FConsoleCommandExecutionResult result;

            if (arguments.Length == 0)
            {
                logProvider.LogVerbose($"Read console variable \"{variable.Name}\" = \"{variable.GetValue()}\".");

                result = new FConsoleCommandExecutionResult(true, $"\"{variable.Name}\" = \"{variable.GetValue()}\"");
            }
            else
            {
                object previousValue = variable.GetValue();

                if (variable.TrySetValue(arguments[0]))
                {
                    logProvider.LogMessage($"Console variable \"{variable.Name}\" changed from \"{previousValue}\" to \"{variable.GetValue()}\".");

                    result = new FConsoleCommandExecutionResult(true, $"\"{variable.Name}\" = \"{variable.GetValue()}\"");
                }
                else
                {
                    logProvider.LogWarning($"Rejected value \"{arguments[0]}\" for console variable \"{variable.Name}\" of type {variable.ValueType.Name}.");

                    result = new FConsoleCommandExecutionResult(false, $"Invalid value for \"{variable.Name}\"'.");
                }
            }

            OnCommandExecuted?.Invoke(result);

            return result;
        }
        #endregion
    }
}
