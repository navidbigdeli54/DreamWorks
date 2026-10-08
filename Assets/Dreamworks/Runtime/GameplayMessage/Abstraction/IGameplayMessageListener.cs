using System;

namespace DreamMachineGameStudio.DreamWorks.GameplayMessage.Abstraction
{
    internal interface IGameplayMessageListener
    {
        FGameplayMessageListenerHandle Handle { get; }

        Type MessageType { get; }

        void Invoke(object message);
    }
}