using System;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayTags;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayMessage.Abstraction
{
    public interface IGameplayMessageSubSystem
    {
        FGameplayMessageListenerHandle RegisterListener<TMessage>(FGameplayTag channel, Action<TMessage> callback);

        bool UnregisterListener(FGameplayMessageListenerHandle handle);

        void BroadcastMessage<TMessage>(FGameplayTag channel, in TMessage message);
    }
}