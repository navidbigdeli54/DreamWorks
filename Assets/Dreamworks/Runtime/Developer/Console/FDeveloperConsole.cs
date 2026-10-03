using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console
{
    /// <summary>
    /// Provides a centralized implementation of a developer console, enabling the registration, management, and
    /// retrieval of console methods and variables.
    /// </summary>
    /// <remarks>This class serves as a sealed implementation of the <see cref="IDeveloperConsole"/> and <see
    /// cref="IDeveloperConsoleInitializer"/> interfaces. It delegates the management of console methods and variables
    /// to the provided repositories, which must implement <see cref="IConsoleMethodRepository"/> and <see
    /// cref="IConsoleVariableRepository"/>, respectively.  The <see cref="FDeveloperConsole"/> is designed to be
    /// initialized and shut down via the <see cref="IDeveloperConsoleInitializer"/> interface, ensuring proper setup
    /// and teardown of its underlying repositories.</remarks>
    public sealed class FDeveloperConsole : IDeveloperConsole, IDeveloperConsoleInitializer
    {
        #region Properties
        private readonly IConsoleMethodRepository methodRepository;

        private readonly IConsoleVariableRepository variableRepository;
        #endregion

        #region Constructors
        public FDeveloperConsole(IConsoleMethodRepository methodRepository, IConsoleVariableRepository variableRepository)
        {
            this.methodRepository = methodRepository;

            this.variableRepository = variableRepository;
        }
        #endregion

        #region IDeveloperConsoleMethodRepository Implementation
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

        #region IDeveloperConsoleVariableRepository Implementation
        void IConsoleVariableRepository.RegisterVariable(IConsoleVariable variable)
        {
            variableRepository.RegisterVariable(variable);
        }

        void IConsoleVariableRepository.RegisterVariable<TVariableType>(string name, TVariableType defaultValue, string description)
        {
            variableRepository.RegisterVariable<TVariableType>(name, defaultValue, description);
        }

        void IConsoleVariableRepository.UnregisterVariable(string variableName)
        {
            variableRepository.UnregisterVariable(variableName);
        }

        bool IConsoleVariableRepository.TryGetVariable(string name, out IConsoleVariable variable)
        {
            return variableRepository.TryGetVariable(name, out variable);
        }

        bool IConsoleVariableRepository.TryGetVariableValue<TVariableType>(string name, out TVariableType value)
        {
            return variableRepository.TryGetVariableValue<TVariableType>(name, out value);
        }
        #endregion

        #region Public Methods
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