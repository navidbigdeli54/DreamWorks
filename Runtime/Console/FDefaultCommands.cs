using UnityEngine;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Console
{
    public class FDefaultCommands
    {
        [FCommand("time.TimeScale")]
        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;

            FLog.Info(nameof(FDefaultCommands), $"Time.timeScale has bet set to {scale}.");
        }
    }
}
