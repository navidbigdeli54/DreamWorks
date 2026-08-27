using System;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayMessage
{
    public readonly struct FGameplayMessageListenerHandle : IEquatable<FGameplayMessageListenerHandle>
    {
        #region Fields
        internal readonly int Id;
        #endregion

        #region Properties
        internal static FGameplayMessageListenerHandle Invalid => default;

        public bool IsValid => Id != 0;
        #endregion

        #region Constructors
        internal FGameplayMessageListenerHandle(int id)
        {
            Id = id;
        }
        #endregion

        public bool Equals(FGameplayMessageListenerHandle other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is FGameplayMessageListenerHandle other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(FGameplayMessageListenerHandle left,
            FGameplayMessageListenerHandle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FGameplayMessageListenerHandle left,
            FGameplayMessageListenerHandle right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return IsValid ? $"GameplayMessageListenerHandle({Id})" : "GameplayMessageListenerHandle(Invalid)";
        }
    }
}