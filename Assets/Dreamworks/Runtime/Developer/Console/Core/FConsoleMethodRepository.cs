using System;
using System.Reflection;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Core
{
    internal sealed class FConsoleMethodRepository : IConsoleMethodRepository, IDeveloperConsoleInitializer, IConsoleCommandQuery
    {
        #region Fields
        private readonly ILogProvider logProvider;
        #endregion

        #region Properties
        internal Dictionary<string, IConsoleMethod> RegisteredMethods { get; private set; }
        #endregion

        #region Constructors
        internal FConsoleMethodRepository(ILogProvider logProvider)
        {
            this.logProvider = logProvider ?? FDefaultLogger.Instance;

            RegisteredMethods = new Dictionary<string, IConsoleMethod>(StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region IDeveloperConsoleMethodRepository Implementation
        void IConsoleMethodRepository.RegisterMethod(IConsoleMethod method)
        {
            RegisterMethod(method);
        }

        void IConsoleMethodRepository.UnregisterMethod(string method)
        {
            UnregisterMethod(method);
        }

        bool IConsoleMethodRepository.TryGetMethod(string name, out IConsoleMethod method)
        {
            return RegisteredMethods.TryGetValue(name, out method);
        }
        #endregion

        #region IDeveloperConsoleInitialization Implementation
        void IDeveloperConsoleInitializer.Initialize()
        {
            DiscoverStaticCommands();
        }

        void IDeveloperConsoleInitializer.ShutDown()
        {

        }
        #endregion

        #region IDeveloperConsoleCommandQuery Implementation
        IReadOnlyList<FConsoleCommandSuggestion> IConsoleCommandQuery.QuerySuggestions(string text)
        {
            var results = new List<FConsoleCommandSuggestion>();

            foreach (var variable in RegisteredMethods.Values)
            {
                if (variable.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(new FConsoleCommandSuggestion(variable.Name, variable.Description));
                }
            }

            return results;
        }
        #endregion

        #region Private Methods
        private void RegisterMethod(IConsoleMethod method)
        {
            if (method == null)
            {
                logProvider.LogError("Attempted to register a null command.");

                return;
            }

            RegisteredMethods[method.Name] = method;

            logProvider.Log($"\"{method.Name}\" command has been registered.");
        }

        private void UnregisterMethod(string name)
        {
            RegisteredMethods.Remove(name);

            logProvider.Log($"\"{name}\" command has been unregistered.");
        }

        private void DiscoverStaticCommands()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                    foreach (MethodInfo methodInfo in methodInfos)
                    {
                        AConsoleMethodAttribute attribute = methodInfo.GetCustomAttribute<AConsoleMethodAttribute>();
                        if (attribute == null)
                        {
                            continue;
                        }

                        var method = new FConsoleMethod(attribute.Name, attribute.Description, methodInfo);

                        RegisterMethod(method);
                    }
                }
            }
        }
        #endregion
    }
}