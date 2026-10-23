using System;
using DreamMachineGameStudio.DreamWorks.GameplayMessage.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameplayMessage
{
    /// <summary>
    /// Represents a listener for gameplay messages of a specific type.
    /// </summary>
    /// <remarks>This class is used to encapsulate a callback that is invoked when a message of type
    /// <typeparamref name="TMessage"/> is received. It provides a handle for managing the listener and exposes the type
    /// of message it listens for.</remarks>
    /// <typeparam name="TMessage">The type of the message that this listener handles.</typeparam>
    internal sealed class TGameplayMessageListener<TMessage> : IGameplayMessageListener
    {
        #region Fields
        private readonly Action<TMessage> callback;
        #endregion

        #region Properties
        public FGameplayMessageListenerHandle Handle { get; }

        public Type MessageType => typeof(TMessage);
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TGameplayMessageListener{TMessage}"/> class with the specified
        /// handle and callback.
        /// </summary>
        /// <remarks>The <paramref name="callback"/> parameter must not be <see langword="null"/>. Ensure
        /// that the handle provided is valid and properly managed to avoid unintended behavior.</remarks>
        /// <param name="handle">The handle that uniquely identifies the gameplay message listener.</param>
        /// <param name="callback">The callback to invoke when a message of type <typeparamref name="TMessage"/> is received.</param>
        public TGameplayMessageListener(FGameplayMessageListenerHandle handle, Action<TMessage> callback)
        {
            Handle = handle;
            this.callback = callback;
        }
        #endregion

        #region Public Methods
        public void Invoke(object message)
        {
            callback((TMessage)message);
        }
        #endregion
    }
}
