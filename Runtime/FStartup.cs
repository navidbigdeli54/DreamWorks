/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Console;
using DreamMachineGameStudio.Dreamworks.Extension;
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

            RemoveExistedDreamworkInstances();

            new GameObject(nameof(MDreamwork)).AddComponent<MDreamwork>();

            new GameObject(nameof(CConsoleView)).AddComponent<CConsoleView>();

            FServiceLocator.Register<ISceneManagerProxy>(new GameObject(nameof(CSceneManagerProxy)).AddComponent<CSceneManagerProxy>());

            FServiceLocator.Register<IGameController>(new GameObject(nameof(CGameController)).AddComponent<CGameController>());

            Configuration();

            MDreamwork.Instance.StartUp();
        }
        #endregion

        #region Private Methods
        private static void RemoveExistedDreamworkInstances()
        {
            MDreamwork[] existedInstances = Object.FindObjectsOfType<MDreamwork>();
            if (existedInstances != null && existedInstances.IsNotEmpty())
            {
                for (int i = 0; i < existedInstances.Length; ++i)
                {
                    Object.Destroy(existedInstances[i].gameObject);
                }
            }
        }
        #endregion
    }
}