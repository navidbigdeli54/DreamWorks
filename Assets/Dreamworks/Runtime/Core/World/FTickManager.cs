using System;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework;

namespace DreamMachineGameStudio.DreamWorks.Core.World
{
    public class FTickManager
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly IGameWorld gameWorld;

        private List<IGameFrameworkTickable>[] tickGroups;
        #endregion

        #region Constructors
        public FTickManager(IGameWorld gameWorld, ILogProvider logProvider)
        {
            this.gameWorld = gameWorld;

            this.logProvider = logProvider;

            InitializeTickGroups();
        }
        #endregion

        #region Public Methods
        public void Register(IGameFrameworkTickable tickable)
        {
            if (gameWorld.IsDisposed)
            {
                logProvider.LogError("Registering a tickable in a disposed world!");

                return;
            }

            if (tickable == null)
            {
                return;
            }

            ETickGroup group = tickable.TickSetting.TickGroup;

            tickGroups[(int)group].Add(tickable);
        }

        public void Unregister(IGameFrameworkTickable tickable)
        {
            if (gameWorld.IsDisposed)
            {
                logProvider.LogError("Unregistering a tickable in a disposed world!");

                return;
            }

            if (tickable == null)
            {
                return;
            }

            ETickGroup group = tickable.TickSetting.TickGroup;

            tickGroups[(int)group].Remove(tickable);
        }

        public void Tick(FFrameContext context)
        {
            float deltaTime = context.DeltaTime;

            for (int groupIndex = 0; groupIndex < tickGroups.Length; ++groupIndex)
            {
                List<IGameFrameworkTickable> group = tickGroups[groupIndex];

                for (int i = 0; i < group.Count; ++i)
                {
                    try
                    {
                        IGameFrameworkTickable tickable = group[i];

                        if (tickable.TickSetting.CanTick == false)
                        {
                            continue;
                        }

                        if (tickable.TickSetting.TickInterval > 0)
                        {
                            tickable.TickState.AccumulatedTime += tickable.TickSetting.TickInterval;

                            while (tickable.TickState.AccumulatedTime >= tickable.TickSetting.TickInterval)
                            {
                                tickable.TickState.AccumulatedTime -= deltaTime;

                                tickable.Tick(deltaTime);
                            }

                            continue;
                        }

                        tickable.Tick(deltaTime);
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError(exception.ToString());
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private void InitializeTickGroups()
        {
            int groupCount = Enum.GetValues(typeof(ETickGroup)).Length;

            tickGroups = new List<IGameFrameworkTickable>[groupCount];

            for (int i = 0; i < groupCount; ++i)
            {
                tickGroups[i] = new List<IGameFrameworkTickable>();
            }
        }
        #endregion
    }
}