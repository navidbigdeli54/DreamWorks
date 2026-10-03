using System;
using System.Linq;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Core
{

    internal sealed class FConsoleCommandActivator : IConsoleCommandActivator
    {
        #region Fields
        private readonly IConsoleMethodRepository methodRepository;

        private readonly IConsoleVariableRepository variableRepository;
        #endregion

        #region Events
        public event Action<string> OnCommandEntered;

        public event Action<FConsoleCommandExecutionResult> OnCommandExecuted;
        #endregion

        #region Constructors
        public FConsoleCommandActivator(IConsoleMethodRepository methodRepository, IConsoleVariableRepository variableRepository)
        {
            this.methodRepository = methodRepository;
            this.variableRepository = variableRepository;
        }
        #endregion

        #region IDeveloperConsoleExecuter Implementation

        FConsoleCommandExecutionResult IConsoleCommandActivator.ExecuteCommand(string commandLine)
        {
            OnCommandEntered?.Invoke(commandLine);

            string[] tokens = FCommandTokenizer.Tokenize(commandLine);

            if (tokens.Length == 0)
            {
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

                var executionResult = new FConsoleCommandExecutionResult(true, $"\"{method.Name}\" method has been executed. result: {result}.");

                OnCommandExecuted?.Invoke(executionResult);

                return executionResult;
            }
            catch (Exception exception)
            {
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
                result = new FConsoleCommandExecutionResult(true, $"\"{variable.Name}\" = \"{variable.GetValue()}\"");
            }
            else
            {
                if (variable.TrySetValue(arguments[0]))
                {
                    result = new FConsoleCommandExecutionResult(true, $"\"{variable.Name}\" = \"{variable.GetValue()}\"");
                }
                else
                {
                    result = new FConsoleCommandExecutionResult(false, $"Invalid value for \"{variable.Name}\"'.");
                }
            }

            OnCommandExecuted?.Invoke(result);

            return result;
        }
        #endregion
    }
}