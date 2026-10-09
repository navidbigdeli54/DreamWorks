using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Core;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console
{
    /// <summary>
    /// Provides unified access to console methods and variables through one gameplay-facing interface.
    /// </summary>
    public sealed class FDeveloperConsole : IDeveloperConsole, IDeveloperConsoleInitializer
    {
        #region Fields
        private readonly IConsoleMethodRepository methodRepository;

        private readonly IConsoleVariableRepository variableRepository;
        #endregion

        #region Properties
        public static IDeveloperConsole Instance { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates the developer console facade from its method and variable repositories.
        /// </summary>
        public FDeveloperConsole(IConsoleMethodRepository methodRepository, IConsoleVariableRepository variableRepository)
        {
            Instance = this;

            this.methodRepository = methodRepository;

            this.variableRepository = variableRepository;
        }
        #endregion

        #region IConsoleMethodRepository Implementation
        void IConsoleMethodRepository.RegisterMethod(IConsoleMethod method)
        {
            methodRepository.RegisterMethod(method);
        }

        void IConsoleMethodRepository.UnregisterMethod(string method)
        {
            methodRepository.UnregisterMethod(method);
        }

        bool IConsoleMethodRepository.TryGetMethod(string name, out IConsoleMethod method)
        {
            return methodRepository.TryGetMethod(name, out method);
        }
        #endregion

        #region IConsoleVariableRepository Implementation
        void IConsoleVariableRepository.RegisterVariable(IConsoleVariable variable)
        {
            variableRepository.RegisterVariable(variable);
        }

        IConsoleVariable IConsoleVariableRepository.RegisterVariable<TVariableType>(string name, TVariableType defaultValue, string description, bool isPersistent)
        {
            return variableRepository.RegisterVariable(name, defaultValue, description, isPersistent);
        }

        void IConsoleVariableRepository.UnregisterVariable(string variableName)
        {
            variableRepository.UnregisterVariable(variableName);
        }

        bool IConsoleVariableRepository.TryGetVariable(string name, out IConsoleVariable variable)
        {
            return variableRepository.TryGetVariable(name, out variable);
        }

        bool IConsoleVariableRepository.TryGetVariable<TVariableType>(string name, out FConsoleVariable<TVariableType> variable)
        {
            return variableRepository.TryGetVariable(name, out variable);
        }

        bool IConsoleVariableRepository.TryGetVariableValue<TVariableType>(string name, out TVariableType value)
        {
            return variableRepository.TryGetVariableValue(name, out value);
        }

        IReadOnlyList<IConsoleVariable> IConsoleVariableRepository.GetRegisteredVariables()
        {
            return variableRepository.GetRegisteredVariables();
        }
        #endregion

        #region IDeveloperConsoleInitializer Implementation
        void IDeveloperConsoleInitializer.Initialize()
        {
            if (methodRepository is IDeveloperConsoleInitializer methodRepositoryInitializer)
            {
                methodRepositoryInitializer.Initialize();
            }

            if (variableRepository is IDeveloperConsoleInitializer variableRepositoryInitializer)
            {
                variableRepositoryInitializer.Initialize();
            }
        }

        void IDeveloperConsoleInitializer.ShutDown()
        {
            if (methodRepository is IDeveloperConsoleInitializer methodRepositoryInitializer)
            {
                methodRepositoryInitializer.ShutDown();
            }

            if (variableRepository is IDeveloperConsoleInitializer variableRepositoryInitializer)
            {
                variableRepositoryInitializer.ShutDown();
            }
        }
        #endregion
    }
}
