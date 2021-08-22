/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable IDE0059

using System;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.EventManager
{
    public static class FEventManager
    {
        #region Fields
        public const string ON_INITIALIZATION = "OnInitialization";
        public const string ON_BEGIN_PLAY = "OnBeginPlay";
        public const string ON_GAME_MODE_LOADED = "OnGameModeLoaded";
        public const string ON_SCENE_LOADED = "OnSceneLoaded";
        public const string ON_SCENE_UNLOADED = "OnSceneUnloaded";
        public const string ON_ACTIVE_SCENE_CHANGED = "OnActiveSceneChanged";

        private static readonly IDictionary<FStringId, DEventSubscriber> eventTosubscribers = new Dictionary<FStringId, DEventSubscriber>(100);
        #endregion

        #region Properties
        public static IReadOnlyDictionary<FStringId, DEventSubscriber> Events => eventTosubscribers as IReadOnlyDictionary<FStringId, DEventSubscriber>;
        #endregion

        #region Methods
        public static void Publish(FStringId name) => Publish(name, EventArgs.Empty);

        public static void Publish(FStringId name, EventArgs args)
        {
            if (eventTosubscribers.TryGetValue(name, out DEventSubscriber subscribers))
            {
                subscribers?.Invoke(args);
            }
        }

        public static void Subscribe(FStringId name, DEventSubscriber subscriber)
        {
            if (eventTosubscribers.ContainsKey(name))
            {
                eventTosubscribers[name] += subscriber;
            }
            else
            {
                eventTosubscribers.Add(name, subscriber);
            }
        }

        public static void Unsubscribe(FStringId name, DEventSubscriber subscriber)
        {
            if (eventTosubscribers.ContainsKey(name))
            {
                eventTosubscribers[name] -= subscriber;
            }
        }
        #endregion

        #region Nested Type
        public delegate void DEventSubscriber(EventArgs eventData);
        #endregion
    }
}