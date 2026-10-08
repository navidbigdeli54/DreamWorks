using System;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.GameplayMessage.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameplayMessage
{
    internal sealed class FGameplayMessageChannel
    {
        #region Fields
        public readonly Type MessageType;

        public readonly List<IGameplayMessageListener> Listeners = new();
        #endregion

        #region Constructors
        public FGameplayMessageChannel(Type messageType)
        {
            MessageType = messageType;
        }
        #endregion
    }
}
