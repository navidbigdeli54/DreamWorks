using System;
using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.GameFramework;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayTags.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayTags
{

    public class CGameplayTagContainerComponent : CGameFrameworkComponent, IGameplayTagContainer
    {
        #region Fields
        [SerializeField]
        private FGameplayTagContainer gameplayTags = new();
        #endregion

        #region Events
        public event Action<FGameplayTag> GameplayTagAdded;

        public event Action<FGameplayTag> GameplayTagRemoved;

        public event Action GameplayTagsCleared;
        #endregion

        #region Public Methods
        public bool AddGameplayTag(FGameplayTag gameplayTag)
        {
            if (!gameplayTag.IsValid)
            {
                return false;
            }

            if (!gameplayTags.AddTag(gameplayTag))
            {
                return false;
            }

            GameplayTagAdded?.Invoke(gameplayTag);

            return true;
        }

        public bool RemoveGameplayTag(FGameplayTag gameplayTag)
        {
            if (!gameplayTags.RemoveTag(gameplayTag))
            {
                return false;
            }

            GameplayTagRemoved?.Invoke(gameplayTag);

            return true;
        }

        public void ClearGameplayTags()
        {
            if (gameplayTags.IsEmpty)
            {
                return;
            }

            IReadOnlyList<FGameplayTag> tags = gameplayTags.GetGameplayTags();

            for (int i = 0; i < tags.Count; ++i)
            {
                GameplayTagRemoved?.Invoke(tags[i]);
            }

            gameplayTags.Clear();

            GameplayTagsCleared?.Invoke();
        }
        #endregion

        #region IGameplayTagContainer Implementation
        bool IGameplayTagContainer.HasGameplayTag(FGameplayTag gameplayTag)
        {
            return gameplayTags.HasTag(gameplayTag);
        }

        bool IGameplayTagContainer.HasAllGameplayTags(FGameplayTagContainer other)
        {
            return gameplayTags.HasAll(other);
        }

        bool IGameplayTagContainer.HasAnyGameplayTags(FGameplayTagContainer other)
        {
            return gameplayTags.HasAny(other);
        }
        #endregion
    }
}