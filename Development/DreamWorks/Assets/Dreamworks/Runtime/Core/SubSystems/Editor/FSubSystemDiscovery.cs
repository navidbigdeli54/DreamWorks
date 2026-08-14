using System;
using System.Linq;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem;

namespace DreamMachineGameStudio.DreamWorks.Editor.SubSystems
{
    public static class FSubSystemDiscovery
    {
        #region Fields
        private static List<Type> existedSubSystems;
        #endregion

        #region Properties
        public static IReadOnlyList<Type> GameInstanceSubsystems => existedSubSystems ??= Discover();
        #endregion

        #region Private Methods
        private static List<Type> Discover()
        {
            Type baseType = typeof(ISubSystem);

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.DefinedTypes; }
                    catch { return Enumerable.Empty<TypeInfo>(); }
                })
                .Where(t =>
                    !t.IsAbstract &&
                    !t.IsInterface &&
                    baseType.IsAssignableFrom(t))
                .Select(t => t.AsType())
                .ToList();
        }
        #endregion
    }
}