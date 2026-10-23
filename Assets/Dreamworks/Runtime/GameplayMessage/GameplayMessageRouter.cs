using System;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.GameplayTags;
using DreamMachineGameStudio.DreamWorks.GameplayMessage.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameplayMessage
{
    /// <summary>
    /// Provides a mechanism for managing gameplay message channels and broadcasting messages to registered listeners.
    /// </summary>
    /// <remarks>The <see cref="FGameplayMessageRouter"/> class allows you to register listeners for specific
    /// gameplay message channels, broadcast messages to those channels, and manage the lifecycle of listeners. Each
    /// channel is identified by a unique <see cref="FGameplayTag"/>, and messages are strongly typed to ensure type
    /// safety.  This class is thread-unsafe and assumes single-threaded usage. It is the caller's responsibility to
    /// ensure thread safety if used in a multithreaded environment.</remarks>
    public sealed class FGameplayMessageRouter
    {
        #region Fields
        private int NextListenerHandleId = 1;

        private readonly Dictionary<FGameplayTag, FGameplayMessageChannel> existedChannels = new();

        private readonly Dictionary<FGameplayMessageListenerHandle, FGameplayTag> channelListeners = new();
        #endregion

        #region Public Methods
        /// <summary>
        /// Registers a listener for messages of the specified type on the given gameplay message channel.
        /// </summary>
        /// <remarks>This method ensures that the specified channel exists and is associated with the
        /// correct message type. If the channel does not exist, it will be created. A listener can only be registered
        /// on a channel for the message type it was originally associated with.</remarks>
        /// <typeparam name="TMessage">The type of message the listener will handle.</typeparam>
        /// <param name="channelTag">The tag identifying the gameplay message channel to listen on. Must be valid.</param>
        /// <param name="callback">The callback to invoke when a message of type <typeparamref name="TMessage"/> is received. Cannot be <see
        /// langword="null"/>.</param>
        /// <returns>A handle that uniquely identifies the registered listener. This handle can be used to manage or unregister
        /// the listener.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="channelTag"/> is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the specified channel is already registered for a different message type.</exception>
        public FGameplayMessageListenerHandle RegisterListener<TMessage>(FGameplayTag channelTag, Action<TMessage> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            if (!channelTag.IsValid)
            {
                throw new ArgumentException("Gameplay message channel must be valid.", nameof(channelTag));
            }

            Type messageType = typeof(TMessage);

            if (!existedChannels.TryGetValue(channelTag, out FGameplayMessageChannel channel))
            {
                channel = new FGameplayMessageChannel(messageType);

                existedChannels.Add(channelTag, channel);
            }

            if (channel.MessageType != messageType)
            {
                throw new InvalidOperationException($"Gameplay message channel '{channelTag}' is already registered " + $"for message type '{channel.MessageType.FullName}', " + $"but attempted to register '{messageType.FullName}'.");
            }

            FGameplayMessageListenerHandle handle = new(NextListenerHandleId++);

            IGameplayMessageListener listener = new TGameplayMessageListener<TMessage>(handle, callback);

            channel.Listeners.Add(listener);

            channelListeners.Add(handle, channelTag);

            return handle;
        }

        /// <summary>
        /// Unregisters a listener from the specified gameplay message channel.
        /// </summary>
        /// <remarks>This method removes the listener associated with the provided handle from its
        /// corresponding gameplay message channel. If the handle is invalid, or if the listener or channel does not
        /// exist, the method returns <see langword="false"/>.</remarks>
        /// <param name="handle">The handle representing the listener to be unregistered. Must be valid.</param>
        /// <returns><see langword="true"/> if the listener was successfully unregistered; otherwise, <see langword="false"/>.</returns>
        public bool UnregisterListener(FGameplayMessageListenerHandle handle)
        {
            if (!handle.IsValid)
            {
                return false;
            }

            if (!channelListeners.TryGetValue(handle, out FGameplayTag channelTag))
            {
                return false;
            }

            channelListeners.Remove(handle);

            if (!existedChannels.TryGetValue(channelTag, out FGameplayMessageChannel channel))
            {
                return false;
            }

            for (int i = 0; i < channel.Listeners.Count; ++i)
            {
                if (channel.Listeners[i].Handle != handle)
                {
                    continue;
                }

                channel.Listeners.RemoveAt(i);

                if (channel.Listeners.Count == 0)
                {
                    existedChannels.Remove(channelTag);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Broadcasts a message to all listeners registered on the specified gameplay message channel.
        /// </summary>
        /// <remarks>If the specified channel does not exist or has no registered listeners, the method
        /// completes without performing any action. Listeners may unregister themselves during the broadcast, and such
        /// changes will not affect the current broadcast iteration.</remarks>
        /// <typeparam name="TMessage">The type of the message being broadcast. This must match the expected message type of the specified channel.</typeparam>
        /// <param name="channel">The gameplay message channel to which the message will be broadcast. Must be a valid channel.</param>
        /// <param name="message">The message to broadcast to all listeners on the specified channel.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="channel"/> is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the specified <paramref name="channel"/> expects a different message type than <typeparamref
        /// name="TMessage"/>.</exception>
        public void BroadcastMessage<TMessage>(FGameplayTag channel, in TMessage message)
        {
            if (!channel.IsValid)
            {
                throw new ArgumentException("Gameplay message channel must be valid.", nameof(channel));
            }

            if (!existedChannels.TryGetValue(channel, out FGameplayMessageChannel channelData))
            {
                return;
            }

            Type messageType = typeof(TMessage);

            if (channelData.MessageType != messageType)
            {
                throw new InvalidOperationException($"Gameplay message channel '{channel}' expects " + $"message type '{channelData.MessageType.FullName}', " + $"but received '{messageType.FullName}'.");
            }

            object boxedMessage = message;

            //Creating a copy of the listeners so they can unregister themself while we are broadcasting this message.
            IGameplayMessageListener[] listeners = channelData.Listeners.ToArray();
            foreach (IGameplayMessageListener listener in listeners)
            {
                // The listener might have been unregistered by an earlier callback in this same broadcast.
                if (!channelListeners.ContainsKey(listener.Handle))
                {
                    continue;
                }

                listener.Invoke(boxedMessage);
            }
        }

        /// <summary>
        /// Removes all channels and their associated listeners from the collection.
        /// </summary>
        /// <remarks>After calling this method, the collection will be empty, and no channels or listeners
        /// will remain.</remarks>
        public void Clear()
        {
            existedChannels.Clear();
            channelListeners.Clear();
        }
        #endregion
    }
}
