using System;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayMessage.Abstraction
{
    internal interface IGameplayMessageListener
    {
        FGameplayMessageListenerHandle Handle { get; }

        Type MessageType { get; }

        void Invoke(object message);
    }
}