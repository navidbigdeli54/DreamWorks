using System;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayTags.Definitions
{
    [Serializable]
    public readonly struct FGameplayTagDefinition
    {
        #region Properties
        public string Name { get; }

        public string Description { get; }
        #endregion

        #region Constructors
        public FGameplayTagDefinition(string name, string description = "")
        {
            Name = name?.Trim();

            Description = description ?? string.Empty;
        }
        #endregion
    }
}