using System;
using UnityEngine;
using System.Threading.Tasks;
using DreamMachineGameStudio.DreamWorks.Extensions;
using DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;

namespace DreamMachineGameStudio.DreamWorks.ResourceProvider
{
    public class FResourceProvider : IResourceProvider
    {
        #region Fields
        private readonly ILogProvider logProvider;
        #endregion

        #region Constructors
        public FResourceProvider(ILogProvider logProvider)
        {
            this.logProvider = logProvider;
        }
        #endregion

        #region IResourceProvider Implementation
        TObject IResourceProvider.LoadResource<TObject>(IResourceKey key)
        {
            string path = ((FResourcesKey)key).ResourcesPath;

            return Resources.Load<TObject>(path);
        }

        Task<TObject> IResourceProvider.LoadResourceAsync<TObject>(IResourceKey key)
        {
            string path = ((FResourcesKey)key).ResourcesPath;

            return Resources.LoadAsync<TObject>(path).GetTask<TObject>();
        }
        #endregion
    }
}
