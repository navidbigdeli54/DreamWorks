using DreamMachineGameStudio.DreamWorks.Modules.GameplayMessage.Abstraction;
using System;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayMessage
{
    internal sealed class TGameplayMessageListener<TMessage> : IGameplayMessageListener
    {
        #region Fields
        private readonly Action<TMessage> Callback;
        #endregion

        #region Properties
        public FGameplayMessageListenerHandle Handle { get; }

        public Type MessageType => typeof(TMessage);
        #endregion

        #region Constructors
        public TGameplayMessageListener(FGameplayMessageListenerHandle handle, Action<TMessage> callback)
        {
            Handle = handle;
            Callback = callback;
        }
        #endregion

        #region Public Methods
        public void Invoke(object message)
        {
            Callback((TMessage)message);
        }
        #endregion
    }
}
