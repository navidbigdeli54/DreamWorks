/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public class FStartup : IStartup
    {
        #region Method
        protected virtual void Configuration() { }
        #endregion

        #region IStartup Implementation
        void IStartup.Run()
        {
            Screen.sleepTimeout = 0;
            Application.targetFrameRate = 60;

            MDreamwork[] existedInstances = Object.FindObjectsOfType<MDreamwork>();
            if (existedInstances != null && existedInstances.Length > 0)
            {
                for (int i = 0; i < existedInstances.Length; ++i)
                {
                    Object.Destroy(existedInstances[i].gameObject);
                }
            }

            new GameObject().AddComponent<MDreamwork>();

            new GameObject().AddComponent<CGameManagement>();

            Configuration();

            MDreamwork.Instance.StartUp();
        }
        #endregion
    }
}