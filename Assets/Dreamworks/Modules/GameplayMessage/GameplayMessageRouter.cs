using System;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayTags;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayMessage.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayMessage
{
    public sealed class FGameplayMessageRouter
    {
        #region Fields
        private int NextListenerHandleId = 1;

        private readonly Dictionary<FGameplayTag, FGameplayMessageChannel> existedChannels = new();

        private readonly Dictionary<FGameplayMessageListenerHandle, FGameplayTag> channelListeners = new();
        #endregion

        #region Public Methods
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

        public void Clear()
        {
            existedChannels.Clear();
            channelListeners.Clear();
        }
        #endregion
    }
}
