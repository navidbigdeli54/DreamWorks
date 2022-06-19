/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;
using DreamMachineGameStudio.Dreamworks.SceneManager;
using DreamMachineGameStudio.Dreamworks.ServiceLocator;

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

            FServiceLocator.Register<IGameManagement>(new GameObject(nameof(CGameManagement)).AddComponent<CGameManagement>());

            FServiceLocator.Register<ISceneManagerProxy>(new GameObject(nameof(CSceneManagerProxy)).AddComponent<CSceneManagerProxy>());

            Configuration();

            MDreamwork.Instance.StartUp();
        }
        #endregion
    }
}