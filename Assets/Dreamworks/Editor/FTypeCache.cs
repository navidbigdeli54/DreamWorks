using System;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Editor
{
    public static class FTypeCache
    {
        #region Fields
        private static readonly Dictionary<Type, List<Type>> CachedDerivedTypes = new();
        #endregion

        #region Public Methods
        public static IReadOnlyList<Type> GetDerivedTypes(Type baseType)
        {
            if (CachedDerivedTypes.TryGetValue(baseType, out List<Type> types))
            {
                return types;
            }

            List<Type> result = new();

            if (baseType.IsClass && !baseType.IsAbstract)
            {
                result.Add(baseType);
            }

            result.AddRange(
                TypeCache.GetTypesDerivedFrom(baseType)
                    .Where(t => !t.IsAbstract && !t.IsInterface)
            );

            result = result
                .Distinct()
                .OrderBy(t => t.Name)
                .ToList();

            CachedDerivedTypes[baseType] = result;

            return result;
        }
        #endregion
    }
}
