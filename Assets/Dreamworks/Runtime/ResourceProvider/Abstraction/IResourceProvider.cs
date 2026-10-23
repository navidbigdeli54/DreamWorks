using System.Threading.Tasks;

namespace DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction
{
    /// <summary>
    /// Defines methods for loading resources synchronously and asynchronously using a specified resource key.
    /// </summary>
    /// <remarks>This interface provides a mechanism to retrieve resources of a specified type, either
    /// synchronously or asynchronously. Implementations of this interface are expected to handle resource management
    /// and retrieval based on the provided resource key.</remarks>
    public interface IResourceProvider
    {
        /// <summary>
        /// Loads a resource of the specified type from the given resource key.
        /// </summary>
        /// <remarks>This method is typically used to load assets such as textures, prefabs, or other
        /// Unity objects from a resource management system. Ensure that the resource key corresponds to a valid
        /// resource of the specified type.</remarks>
        /// <typeparam name="TObject">The type of the resource to load. Must derive from <see cref="UnityEngine.Object"/>.</typeparam>
        /// <param name="key">The key identifying the resource to load.</param>
        /// <returns>The loaded resource of type <typeparamref name="TObject"/> if found; otherwise, <see langword="null"/>.</returns>
        TObject LoadResource<TObject>(IResourceKey key) where TObject : UnityEngine.Object;

        /// <summary>
        /// Asynchronously loads a resource of the specified type using the provided resource key.
        /// </summary>
        /// <remarks>This method is typically used to load resources such as assets or prefabs in Unity.
        /// Ensure that the resource key corresponds to a valid resource.</remarks>
        /// <typeparam name="TObject">The type of the resource to load. Must derive from <see cref="UnityEngine.Object"/>.</typeparam>
        /// <param name="key">The key identifying the resource to load.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the loaded resource of type
        /// <typeparamref name="TObject"/>,  or <see langword="null"/> if the resource could not be found.</returns>
        Task<TObject> LoadResourceAsync<TObject>(IResourceKey key) where TObject : UnityEngine.Object;
    }
}
