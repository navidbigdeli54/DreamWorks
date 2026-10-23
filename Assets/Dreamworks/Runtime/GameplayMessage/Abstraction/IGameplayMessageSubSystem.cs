using System;
using DreamMachineGameStudio.DreamWorks.GameplayTags;

namespace DreamMachineGameStudio.DreamWorks.GameplayMessage.Abstraction
{
    /// <summary>
    /// Defines a system for registering, unregistering, and broadcasting gameplay messages across specified channels.
    /// </summary>
    /// <remarks>This interface provides a mechanism for subscribing to and broadcasting messages within a
    /// gameplay system. Messages are categorized by channels, represented by <see cref="FGameplayTag"/>, and can carry
    /// data of a specified type.</remarks>
    public interface IGameplayMessageSubSystem
    {
        #region Methods
        /// <summary>
        /// Registers a listener for messages of the specified type on the given channel.
        /// </summary>
        /// <remarks>This method allows you to subscribe to messages of a specific type on a particular
        /// channel.  The <paramref name="callback"/> will be invoked whenever a message of type <typeparamref
        /// name="TMessage"/>  is received on the specified channel. Ensure that the channel is valid and that the
        /// callback is properly  implemented to handle the message.</remarks>
        /// <typeparam name="TMessage">The type of message to listen for.</typeparam>
        /// <param name="channel">The channel to which the listener will subscribe. Must be a valid <see cref="FGameplayTag"/>.</param>
        /// <param name="callback">The action to invoke when a message of type <typeparamref name="TMessage"/> is received on the specified
        /// channel. Cannot be null.</param>
        /// <returns>A handle that can be used to manage the registered listener, such as for unregistration.</returns>
        FGameplayMessageListenerHandle RegisterListener<TMessage>(FGameplayTag channel, Action<TMessage> callback);

        /// <summary>
        /// Unregisters a previously registered listener for gameplay messages.
        /// </summary>
        /// <param name="handle">The handle representing the listener to be unregistered. This handle must have been obtained from a prior
        /// registration.</param>
        /// <returns><see langword="true"/> if the listener was successfully unregistered; otherwise, <see langword="false"/> if
        /// the handle was invalid or the listener was not found.</returns>
        bool UnregisterListener(FGameplayMessageListenerHandle handle);

        /// <summary>
        /// Broadcasts a message to all listeners subscribed to the specified channel.
        /// </summary>
        /// <remarks>This method sends the specified message to all listeners that are actively subscribed
        /// to the given channel.  Ensure that the channel is properly registered and that listeners are subscribed
        /// before invoking this method.</remarks>
        /// <typeparam name="TMessage">The type of the message being broadcast.</typeparam>
        /// <param name="channel">The channel to which the message will be broadcast. Must be a valid <see cref="FGameplayTag"/>.</param>
        /// <param name="message">The message to broadcast. The message is passed by reference to ensure efficient handling of large data
        /// structures.</param>
        void BroadcastMessage<TMessage>(FGameplayTag channel, in TMessage message); 
        #endregion
    }
}