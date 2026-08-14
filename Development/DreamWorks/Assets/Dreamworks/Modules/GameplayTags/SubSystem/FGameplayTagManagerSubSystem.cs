using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.SubSystems.Attributes;
using DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayTags.Providers;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayTags.Abstraction;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayTags.Definitions;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayTags
{
    [ADreamWorksSubSystem(
        displayName: "Gameplay Tags",
        description: "Provides registration, lookup, validation, and hierarchical queries for gameplay tags used throughout the Game Framework.",
        category: "Game Framework",
        order: 10,
        Experimental = false,
        Advanced = false,
        Keywords = "gameplay tags tag tagging hierarchy query ability gas state attribute gameplay framework")]
    public sealed class FGameplayTagManagerSubSystem : FGameInstanceSubSystem, IGameplayTagManagerSubSystem
    {
        #region Fields
        private readonly Dictionary<string, FGameplayTagDefinition> definitions = new(StringComparer.Ordinal);

        private readonly Dictionary<string, FGameplayTag> tags = new(StringComparer.Ordinal);
        #endregion

        #region Properties
        public override Type RegistrationType => typeof(IGameplayTagManagerSubSystem);
        #endregion

        #region Constructors
        public FGameplayTagManagerSubSystem(IGameInstance gameInstance)
            : base(gameInstance)
        {
        }
        #endregion

        #region SubSystem Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            IGameplayTagSource source = new FReflectionGameplayTagSource();

            foreach (FGameplayTagDefinition definition in source.GetGameplayTags())
            {
                Register(definition);
            }
        }

        protected override async Task ShutDownAsync()
        {
            await base.ShutDownAsync();

            definitions.Clear();

            tags.Clear();
        }
        #endregion

        #region IGameplayTagSubSystem Implementation
        bool IGameplayTagManagerSubSystem.IsRegistered(FGameplayTag gameplayTag)
        {
            return gameplayTag.IsValid && tags.ContainsKey(gameplayTag.TagName);
        }

        FGameplayTag IGameplayTagManagerSubSystem.RequestGameplayTag(string tagName)
        {
            if (!tags.TryGetValue(tagName, out FGameplayTag gameplayTag))
            {
                throw new InvalidOperationException($"Gameplay Tag '{tagName}' is not registered.");
            }

            return gameplayTag;
        }

        IReadOnlyCollection<FGameplayTag> IGameplayTagManagerSubSystem.GetRegisteredGameplayTags()
        {
            return tags.Values.ToArray();
        }
        #endregion

        #region Private Methods
        private void Register(FGameplayTagDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(definition.Name))
            {
                return;
            }

            if (definitions.ContainsKey(definition.Name))
            {
                throw new InvalidOperationException(
                    $"Gameplay Tag '{definition.Name}' is already registered.");
            }

            definitions.Add(definition.Name, definition);

            tags.Add(definition.Name, new FGameplayTag(definition.Name));
        }
        #endregion
    }
}
