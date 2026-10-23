using System;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.GameplayMessage.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameplayMessage
{
    /// <summary>
    /// Represents a communication channel for dispatching and receiving gameplay messages of a specific type.
    /// </summary>
    /// <remarks>This class is used to manage a collection of listeners that handle messages of the specified
    /// type. It ensures that messages are routed to all registered listeners. The type of messages handled by the
    /// channel is defined at the time of construction.</remarks>
    internal sealed class FGameplayMessageChannel
    {
        #region Fields
        /// <summary>
        /// Represents the type of the message associated with this instance.
        /// </summary>
        public readonly Type MessageType;

        /// <summary>
        /// A read-only collection of listeners that implement the <see cref="IGameplayMessageListener"/> interface.
        /// </summary>
        /// <remarks>This collection is used to store instances of objects that listen for gameplay
        /// messages.  Items can be added to or removed from the collection, but the reference to the collection itself
        /// cannot be reassigned.</remarks>
        public readonly List<IGameplayMessageListener> Listeners = new();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FGameplayMessageChannel"/> class with the specified message
        /// type.
        /// </summary>
        /// <param name="messageType">The type of messages that this channel will handle. This parameter cannot be <see langword="null"/>.</param>
        public FGameplayMessageChannel(Type messageType)
        {
            MessageType = messageType;
        }
        #endregion
    }
}
