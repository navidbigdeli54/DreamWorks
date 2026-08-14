using System;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework
{
    [Serializable]
    public class FTickSetting
    {
        public ETickGroup TickGroup = ETickGroup.Gameplay;

        public bool CanTick = false;

        public float TickInterval = 0;
    }
}