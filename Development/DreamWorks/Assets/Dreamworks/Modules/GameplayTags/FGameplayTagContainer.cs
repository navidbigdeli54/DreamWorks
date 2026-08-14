using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayTags
{
    [Serializable]
    public sealed class FGameplayTagContainer : IEnumerable<FGameplayTag>
    {
        #region Fields
        [SerializeField]
        private readonly List<FGameplayTag> gameplayTags = new();
        #endregion

        #region Properties
        public int Count => gameplayTags.Count;

        public bool IsEmpty => gameplayTags.Count == 0;
        #endregion

        #region Public Methods
        public bool AddTag(FGameplayTag gameplayTag)
        {
            if (gameplayTag.IsValid == false)
            {
                return false;
            }

            if (gameplayTags.Contains(gameplayTag))
            {
                return false;
            }

            gameplayTags.Add(gameplayTag);

            return true;
        }

        public bool RemoveTag(FGameplayTag gameplayTag)
        {
            return gameplayTags.Remove(gameplayTag);
        }

        public void Clear()
        {
            gameplayTags.Clear();
        }

        public bool HasTag(FGameplayTag gameplayTag)
        {
            for (int i = 0; i < gameplayTags.Count; ++i)
            {
                if (gameplayTags[i] == gameplayTag)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasAll(FGameplayTagContainer other)
        {
            if (other == null)
            {
                return false;
            }

            for (int i = 0; i < other.gameplayTags.Count; ++i)
            {
                if (!HasTag(other.gameplayTags[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public bool HasAny(FGameplayTagContainer other)
        {
            if (other == null)
            {
                return false;
            }

            for (int i = 0; i < other.gameplayTags.Count; ++i)
            {
                if (HasTag(other.gameplayTags[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public IReadOnlyList<FGameplayTag> GetGameplayTags()
        {
            return gameplayTags;
        }
        #endregion

        #region IEnumerable
        public IEnumerator<FGameplayTag> GetEnumerator()
        {
            return gameplayTags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
