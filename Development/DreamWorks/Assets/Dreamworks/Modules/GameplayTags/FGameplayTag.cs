using System;
using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayTags
{

    [Serializable]
    public readonly struct FGameplayTag : IEquatable<FGameplayTag>, IComparable<FGameplayTag>
    {
        #region Fields
        [SerializeField]
        private readonly string tagName;
        #endregion

        #region Properties
        public string TagName => tagName;

        public bool IsValid => string.IsNullOrWhiteSpace(tagName) == false;
        #endregion

        #region Constructors
        public FGameplayTag(string tagName)
        {
            this.tagName = tagName?.Trim();
        }
        #endregion

        #region Public Methods
        public bool Matches(FGameplayTag other)
        {
            return tagName == other.tagName;
        }

        public bool Matches(string otherTag)
        {
            return tagName == otherTag;
        }
        #endregion

        #region Object
        public override string ToString()
        {
            return tagName ?? string.Empty;
        }

        public override int GetHashCode()
        {
            return tagName?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            return obj is FGameplayTag other && Equals(other);
        }
        #endregion

        #region IEquatable
        public bool Equals(FGameplayTag other)
        {
            return string.Equals(tagName, other.tagName, StringComparison.Ordinal);
        }
        #endregion

        #region IComparable
        public int CompareTo(FGameplayTag other)
        {
            return string.Compare(tagName, other.tagName, StringComparison.Ordinal);
        }
        #endregion

        #region Operators
        public static bool operator ==(FGameplayTag left, FGameplayTag right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FGameplayTag left, FGameplayTag right)
        {
            return left.Equals(right) == false;
        }

        public static implicit operator string(FGameplayTag tag)
        {
            return tag.tagName;
        }

        public static explicit operator FGameplayTag(string tagName)
        {
            return new(tagName);
        }
        #endregion
    }
}
