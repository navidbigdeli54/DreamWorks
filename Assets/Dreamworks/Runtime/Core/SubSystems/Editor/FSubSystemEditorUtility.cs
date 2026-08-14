using System;
using System.Reflection;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.World.SubSystems;
using DreamMachineGameStudio.DreamWorks.Core.SubSystems.Attributes;
using DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems;

namespace DreamMachineGameStudio.DreamWorks.Editor.SubSystems
{
    internal static class FSubSystemEditorUtility
    {
        #region Fields
        private static Dictionary<Type, FSubSystemMetadata> existedMetaDatas = new Dictionary<Type, FSubSystemMetadata>();
        #endregion

        #region Public Methods
        public static FSubSystemMetadata GetMetadata(Type type)
        {
            if (existedMetaDatas.TryGetValue(type, out FSubSystemMetadata metaData))
            {
                return metaData;
            }

            metaData = CreateMetaData(type);

            existedMetaDatas.Add(type, metaData);

            return metaData;
        }

        public static string GetDisplayName(Type type)
        {
            return GetMetadata(type).DisplayName;
        }

        public static string GetTagString(Type type)
        {
            FSubSystemMetadata metaData = GetMetadata(type);

            string tags = metaData.Category;

            if (metaData.Experimental) tags += " | Experimental";

            if (metaData.Advanced) tags += " | Advanced";

            if (typeof(FGameInstanceSubSystem).IsAssignableFrom(type))
                tags += " | GameInstance";

            if (typeof(FGameWorldSubSystem).IsAssignableFrom(type))
                tags += " | GameWorld";

            return tags;
        }

        public static bool MatchesSearch(Type type, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return true;
            }

            FSubSystemMetadata metaData = GetMetadata(type);

            search = search.ToLowerInvariant();

            return metaData.DisplayName.Contains(search, StringComparison.OrdinalIgnoreCase)
                || metaData.Category.Contains(search, StringComparison.OrdinalIgnoreCase)
                || metaData.Keywords.Contains(search, StringComparison.OrdinalIgnoreCase)
                || type.Name.Contains(search, StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        private static FSubSystemMetadata CreateMetaData(Type type)
        {
            ADreamWorksSubSystemAttribute attribute = type.GetCustomAttribute<ADreamWorksSubSystemAttribute>();

            FSubSystemMetadata metaData = new FSubSystemMetadata(
                type,
                attribute?.DisplayName ?? Nicify(type.Name),
                attribute?.Description ?? string.Empty,
                attribute?.Category ?? "Default",
                attribute?.Experimental ?? false,
                attribute?.Advanced ?? false,
                attribute?.Order ?? 0,
                attribute?.Keywords ?? string.Empty
            );
            return metaData;
        }

        private static string Nicify(string name)
        {
            return UnityEditor.ObjectNames.NicifyVariableName(name);
        }
    }
}