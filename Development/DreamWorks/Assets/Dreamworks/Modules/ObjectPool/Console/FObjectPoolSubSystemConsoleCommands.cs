using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Core;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Log;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes;
using DreamMachineGameStudio.DreamWorks.Modules.ObjectPool.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Modules.ObjectPool.Console
{
    public static class FObjectPoolSubSystemConsoleCommands
    {
        [AConsoleCommand("PrintObjectPoolStat")]
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