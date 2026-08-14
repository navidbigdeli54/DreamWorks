using System;
using UnityEngine;
using DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.ResourceProvider
{
    [Serializable]
    public class FResourcesKey : IResourceKey
    {
        #region Properties
        [field: SerializeField]
        public string ResourcesPath { get; private set; }
        #endregion

        #region Constructors
        public FResourcesKey(string resourcesPath)
        {
            ResourcesPath = resourcesPath;
        }
        #endregion
    }

    [Serializable]
    public class FResourcesKey<TObject> : FResourcesKey
    {
        #region Constructors
        public FResourcesKey(string resourcesPath)
            : base(resourcesPath)
        {
        }
        #endregion
    }
}
