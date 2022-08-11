/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Console;
using DreamMachineGameStudio.Dreamworks.SceneManager;
using DreamMachineGameStudio.Dreamworks.ServiceLocator;

namespace DreamMachineGameStudio.Dreamworks
{
    public class FStartup : IStartup
    {
        #region Method
        protected virtual void Configuration() { }
        #endregion

        #region IStartup Implementation
        void IStartup.Run()
        {
            MDreamwork[] existedInstances = Object.FindObjectsOfType<MDreamwork>();
            if (existedInstances != null && existedInstances.Length > 0)
            {
                for (int i = 0; i < existedInstances.Length; ++i)
                {
                    Object.Destroy(existedInstances[i].gameObject);
                }
            }

            new GameObject(nameof(MDreamwork)).AddComponent<MDreamwork>();

            new GameObject(nameof(CConsoleView)).AddComponent<CConsoleView>();

            FServiceLocator.Register<IGameManagement>(new GameObject(nameof(CGameManagement)).AddComponent<CGameManagement>());

            FServiceLocator.Register<ISceneManagerProxy>(new GameObject(nameof(CSceneManagerProxy)).AddComponent<CSceneManagerProxy>());

            Configuration();

            MDreamwork.Instance.StartUp();
        }
        #endregion
    }
}