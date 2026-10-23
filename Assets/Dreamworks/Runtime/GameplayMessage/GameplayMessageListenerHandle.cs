using System;

namespace DreamMachineGameStudio.DreamWorks.GameplayMessage
{
    /// <summary>
    /// Represents a handle to a gameplay message listener, used to uniquely identify and manage a listener instance.
    /// </summary>
    /// <remarks>This struct provides a lightweight, immutable identifier for a gameplay message listener. It
    /// can be used to check the validity of the listener, compare handles, and retrieve a string representation of the
    /// handle. A handle is considered valid if it has a non-zero identifier.</remarks>
    public readonly struct FGameplayMessageListenerHandle : IEquatable<FGameplayMessageListenerHandle>
    {
        #region Fields
        /// <summary>
        /// Represents the unique identifier for the instance.
        /// </summary>
        internal readonly int Id;
        #endregion

        #region Properties
        /// <summary>
        /// Represents an invalid or uninitialized gameplay message listener handle.
        /// </summary>
        internal static FGameplayMessageListenerHandle Invalid => default;

        /// <summary>
        /// Gets a value indicating whether the current object is valid.
        /// </summary>
        public bool IsValid => Id != 0;
        #endregion

        #region Constructors
        /// <summary>
        /// Represents a handle used to identify and manage a gameplay message listener.
        /// </summary>
        /// <remarks>This handle is typically used internally to track and manage the lifecycle of a
        /// gameplay message listener.</remarks>
        /// <param name="id">The unique identifier for the gameplay message listener.</param>
        internal FGameplayMessageListenerHandle(int id)
        {
            Id = id;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Determines whether the current instance is equal to the specified <see
        /// cref="FGameplayMessageListenerHandle"/>.
        /// </summary>
        /// <param name="other">The <see cref="FGameplayMessageListenerHandle"/> to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified <see cref="FGameplayMessageListenerHandle"/> is equal to the current
        /// instance; otherwise, <see langword="false"/>.</returns>
        public bool Equals(FGameplayMessageListenerHandle other)
        {
            return Id == other.Id;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified object is of type <see cref="FGameplayMessageListenerHandle"/>  and
        /// is equal to the current instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is FGameplayMessageListenerHandle other &&
                   Equals(other);
        }

        /// <summary>
        /// Returns the hash code for the current object.
        /// </summary>
        /// <remarks>The hash code is derived from the <see cref="Id"/> property.  Ensure that the <see
        /// cref="Id"/> value remains consistent while the object is used as a key in a hash-based collection.</remarks>
        /// <returns>An integer representing the hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return Id;
        }

        /// <summary>
        /// Returns a string representation of the current <see cref="GameplayMessageListenerHandle"/>.
        /// </summary>
        /// <returns>A string in the format "GameplayMessageListenerHandle({Id})" if the handle is valid;  otherwise,
        /// "GameplayMessageListenerHandle(Invalid)".</returns>
        public override string ToString()
        {
            return IsValid ? $"GameplayMessageListenerHandle({Id})" : "GameplayMessageListenerHandle(Invalid)";
        }
        #endregion

        #region Operator Overrides
        /// <summary>
        /// Determines whether two <see cref="FGameplayMessageListenerHandle"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="FGameplayMessageListenerHandle"/> to compare.</param>
        /// <param name="right">The second <see cref="FGameplayMessageListenerHandle"/> to compare.</param>
        /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(FGameplayMessageListenerHandle left, FGameplayMessageListenerHandle right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="FGameplayMessageListenerHandle"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="FGameplayMessageListenerHandle"/> to compare.</param>
        /// <param name="right">The second <see cref="FGameplayMessageListenerHandle"/> to compare.</param>
        /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(FGameplayMessageListenerHandle left, FGameplayMessageListenerHandle right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}