using System;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayMessage.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayMessage
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
