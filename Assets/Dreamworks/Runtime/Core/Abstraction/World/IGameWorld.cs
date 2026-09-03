using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework;

using UObject = UnityEngine.Object;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction
{
    public interface IGameWorld
    {
        #region Properties
        Scene PrimaryScene { get; }

        bool HasBegunPlay { get; }

        bool IsDisposed { get; }

        IGameMode GameMode { get; }

        IGameInstance GameInstance { get; }
        #endregion

        #region SubSystem API
        TSubSystem GetSubSystem<TSubSystem>();
        #endregion

        #region Object API
        TComponent SpawnGameObject<TComponent>(TComponent prefab, Vector3 position, Quaternion rotation) where TComponent : UObject;

        GameObject SpawnGameObject(GameObject prefab, Vector3 position, Quaternion rotation);

        IReadOnlyList<TComponent> FindComponents<TComponent>() where TComponent : IGameFrameworkComponent;

        TComponent FindComponent<TComponent>() where TComponent : IGameFrameworkComponent;

        void Destroy(GameObject gameObject);
        #endregion

        #region Scene API
        bool HasAnyScene();

        bool ContainsScene(Scene scene);

        void AddScene(Scene scene);

        void RemoveScene(Scene scene);
        #endregion
    }
}