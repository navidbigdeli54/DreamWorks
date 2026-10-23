using System;
using DreamMachineGameStudio.DreamWorks.GameplayTags;
using DreamMachineGameStudio.DreamWorks.SubSystems.Attributes;
using DreamMachineGameStudio.DreamWorks.GameInstance.SubSystems;
using DreamMachineGameStudio.DreamWorks.GameInstance.Abstraction;
using DreamMachineGameStudio.DreamWorks.GameplayMessage.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameplayMessage
{
    /// <summary>
    /// Provides channel-based messaging for decoupled communication between gameplay systems.
    /// </summary>
    /// <remarks>This subsystem facilitates communication between gameplay systems by allowing messages to be
    /// broadcasted on specific channels and listened to by registered listeners. It is designed to promote loose
    /// coupling between systems, enabling more modular and maintainable game architecture.  The subsystem implements
    /// the <see cref="IGameplayMessageSubSystem"/> interface, providing methods to register listeners, unregister
    /// listeners, and broadcast messages. Messages are routed through an internal message router, ensuring efficient
    /// delivery to the appropriate listeners.  This subsystem is registered as a game instance subsystem and is
    /// automatically initialized and shut down with the game instance lifecycle.</remarks>
    [ADreamWorksSubSystem(
        displayName: "Gameplay Message",
        description: "Provides channel-based messaging for decoupled communication between gameplay systems.",
        category: "Game Framework",
        order: 20,
        Experimental = false,
        Advanced = false,
        Keywords = "gameplay message messaging event bus channel broadcast listener communication")]
    public sealed class FGameplayMessageSubSystem : FGameInstanceSubSystem, IGameplayMessageSubSystem
    {
        #region Fields
        private readonly FGameplayMessageRouter MessageRouter = new();
        #endregion

        #region Properties
        public override Type RegistrationType => typeof(IGameplayMessageSubSystem);
        #endregion

        #region Constructors
        public FGameplayMessageSubSystem(IGameInstance gameInstance)
            : base(gameInstance)
        {

        }
        #endregion

        #region FGameInstanceSubSystem Methods
        protected override void ShutDown()
        {
            base.ShutDown();

            MessageRouter.Clear();
        }
        #endregion

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