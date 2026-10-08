using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Core;
using DreamMachineGameStudio.DreamWorks.ObjectPool.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes;

namespace DreamMachineGameStudio.DreamWorks.ObjectPool.Console
{
    public static class FObjectPoolSubSystemConsoleCommands
    {
        [AConsoleMethod("PrintObjectPoolStat")]
        public static string GetObjectPoolStat()
        {
            FObjectPoolSubSystem objectPoolSubSystem = (FObjectPoolSubSystem)FGame.Instance.GameInstance.GetSubSystem<IObjectPoolSubSystem>();

            string result = string.Empty;

            foreach (KeyValuePair<EntityId, FObjectPool> pair in objectPoolSubSystem.ObjectPools)
            {
                FObjectPool pool = pair.Value;

                result += $"Pool: {pool.Prefab.GameObject.name}, Available: {pool.AvailableCount}, Active: {pool.ActiveCount}, Total: {pool.TotalCount} \n";
            }

            ILogProvider logProvider = new FScopedLogger(new FLogCategory(nameof(IObjectPoolSubSystem), ELogVerbosity.Display));

            logProvider.Log(result);

            return result;
        }
    }
}