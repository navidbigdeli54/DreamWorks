using System;
using DreamMachineGameStudio.DreamWorks.GameplayTags;

namespace DreamMachineGameStudio.DreamWorks.GameplayMessage.Abstraction
{
    public interface IGameplayMessageSubSystem
    {
        FGameplayMessageListenerHandle RegisterListener<TMessage>(FGameplayTag channel, Action<TMessage> callback);

        bool UnregisterListener(FGameplayMessageListenerHandle handle);

        void BroadcastMessage<TMessage>(FGameplayTag channel, in TMessage message);
    }
}