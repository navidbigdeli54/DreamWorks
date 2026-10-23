using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.DreamWorks.Application;
using DreamMachineGameStudio.DreamWorks.GameInstance;
using DreamMachineGameStudio.DreamWorks.GameFramework;


using DreamMachineGameStudio.DreamWorks.GameFramework.GameMode;

using UObject = UnityEngine.Object;
using DreamMachineGameStudio.DreamWorks.GameFramework.GameMode.Abstraction;
using DreamMachineGameStudio.DreamWorks.GameInstance.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.World.Abstraction
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

        #region Object API
        TComponent SpawnGameObject<TComponent>(TComponent prefab, Transform transform) where TComponent : UObject;

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