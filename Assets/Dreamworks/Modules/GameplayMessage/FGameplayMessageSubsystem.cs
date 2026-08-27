using System;
using System.Threading.Tasks;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayTags;
using DreamMachineGameStudio.DreamWorks.Core.SubSystems.Attributes;
using DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayMessage.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayMessage
{
    [ADreamWorksSubSystem(
        displayName: "Gameplay Message",
        description: "Provides channel-based messaging for decoupled communication between gameplay systems.",
        category: "Game Framework",
        order: 20,
        Experimental = false,
        Advanced = false,
        Keywords = "gameplay message messaging event bus channel broadcast listener communication")]
    public sealed class FGameplayMessageSubsystem : FGameInstanceSubSystem, IGameplayMessageSubSystem
    {
        #region Properties
        private FGameplayMessageRouter MessageRouter { get; } = new();
        #endregion

        #region Constructors
        public FGameplayMessageSubsystem(IGameInstance gameInstance)
            : base(gameInstance)
        {

        }
        #endregion

        protected override async Task ShutDownAsync()
        {
            await base.ShutDownAsync();

            MessageRouter.Clear();
        }

        #region IGameplayMessageSubSystem Implementation
        FGameplayMessageListenerHandle IGameplayMessageSubSystem.RegisterListener<TMessage>(FGameplayTag channel, Action<TMessage> callback)
        {
            return MessageRouter.RegisterListener(channel, callback);
        }

        bool IGameplayMessageSubSystem.UnregisterListener(FGameplayMessageListenerHandle handle)
        {
            return MessageRouter.UnregisterListener(handle);
        }

        void IGameplayMessageSubSystem.BroadcastMessage<TMessage>(FGameplayTag channel, in TMessage message)
        {
            MessageRouter.BroadcastMessage(channel, in message);
        }
        #endregion
    } 
}