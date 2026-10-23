using System;

namespace DreamMachineGameStudio.DreamWorks.GameplayMessage.Abstraction
{
    /// <summary>
    /// Represents a listener for gameplay messages, providing a handle to manage the listener and the type of messages
    /// it processes.
    /// </summary>
    /// <remarks>This interface is used to define a contract for handling gameplay messages of a specific
    /// type.  Implementations of this interface should specify how messages are processed when <see cref="Invoke"/> is
    /// called.</remarks>
    internal interface IGameplayMessageListener
    {
        #region Properties
        /// <summary>
        /// Gets the handle associated with the gameplay message listener.
        /// </summary>
        FGameplayMessageListenerHandle Handle { get; }

        /// <summary>
        /// Gets the type of the message associated with this instance.
        /// </summary>
        Type MessageType { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Invokes the specified message, triggering the associated action or processing logic.
        /// </summary>
        /// <param name="message">The message to be processed. Cannot be null.</param>
        void Invoke(object message); 
        #endregion
    }
}