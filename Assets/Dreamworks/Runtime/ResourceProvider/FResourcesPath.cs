using System;
using UnityEngine;
using DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.ResourceProvider
{
    /// <summary>
    /// Represents a resource path within the Unity Resources folder.
    /// </summary>
    /// <remarks>This class provides a way to encapsulate and manage resource paths used to locate assets
    /// within the Unity Resources folder. The path should be relative to the Resources folder and should not include
    /// the file extension.</remarks>
    [Serializable]
    public class FResourcesPath : IResourceKey
    {
        #region Properties
        [field: SerializeField]
        public string ResourcesPath { get; private set; }
        #endregion

        #region Constructors
        public FResourcesPath(string resourcesPath)
        {
            ResourcesPath = resourcesPath;
        }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return ResourcesPath;
        }
        #endregion
    }

    /// <summary>
    /// Represents a strongly-typed path to a resource in the application's resources folder.
    /// </summary>
    /// <remarks>This class extends <see cref="FResourcesPath"/> to provide type safety for resources,
    /// ensuring that the resource path is associated with a specific object type.</remarks>
    /// <typeparam name="TObject">The type of the object associated with the resource path. This allows the resource path to be strongly typed to
    /// the expected resource type.</typeparam>
    [Serializable]
    public class FResourcesPath<TObject> : FResourcesPath
    {
        #region Constructors
        public FResourcesPath(string resourcesPath)
            : base(resourcesPath)
        {
        }
        #endregion
    }
}
