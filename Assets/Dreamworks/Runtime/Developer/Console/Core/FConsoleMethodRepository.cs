using System;
using System.Reflection;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Core
{
    /// <summary>
    /// Provides a repository for managing console methods, including registration, unregistration,  and querying of
    /// available commands. This class also supports initialization and discovery  of static console commands.
    /// </summary>
    /// <remarks>This class is designed to be used as part of a developer console system. It allows for the 
    /// dynamic registration and unregistration of console methods, as well as querying for command  suggestions based
    /// on user input. Static commands are automatically discovered during  initialization by scanning loaded assemblies
    /// for methods annotated with the appropriate  attributes.</remarks>
    internal sealed class FConsoleMethodRepository : IConsoleMethodRepository, IDeveloperConsoleInitializer, IConsoleCommandQuery
    {
        #region Properties
        internal Dictionary<string, IConsoleMethod> RegisteredMethods { get; private set; }
        #endregion

        #region Constructors
        internal FConsoleMethodRepository()
        {
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
                return;
            }

            RegisteredMethods[method.Name] = method;
        }

        private void UnregisterMethod(string name)
        {
            RegisteredMethods.Remove(name);
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