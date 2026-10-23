using System;
using UnityEngine;
using System.Threading.Tasks;
using DreamMachineGameStudio.DreamWorks.Extensions;
using DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.ResourceProvider
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IResourceProvider"/> interface,  enabling the loading of
    /// resources synchronously and asynchronously using a specified resource key.
    /// </summary>
    /// <remarks>This implementation relies on the Unity <see cref="Resources"/> API to load resources  based
    /// on the path provided by the <see cref="IResourceKey"/>. The resource key must be  of type <see
    /// cref="FResourcesPath"/> to extract the resource path.</remarks>
    public class FDefaultResourceProvider : IResourceProvider
    {
        #region Fields
        private readonly ILogProvider logProvider;
        #endregion

        #region Constructors
        public FDefaultResourceProvider(ILogProvider logProvider)
        {
            this.logProvider = logProvider;
        }
        #endregion

        #region IResourceProvider Implementation
        TObject IResourceProvider.LoadResource<TObject>(IResourceKey key)
        {
            string path = ((FResourcesPath)key).ResourcesPath;

            return Resources.Load<TObject>(path);
        }

        Task<TObject> IResourceProvider.LoadResourceAsync<TObject>(IResourceKey key)
        {
            string path = ((FResourcesPath)key).ResourcesPath;

            return Resources.LoadAsync<TObject>(path).GetTask<TObject>();
        }
        #endregion
    }
}
