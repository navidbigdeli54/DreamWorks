using DreamMachineGameStudio.DreamWorks.LogProvider.Definitions.Persitence;

namespace DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction.Persistence
{
    /// <summary>
    /// Defines the contract for managing the persistence of log category data.
    /// </summary>
    /// <remarks>This interface provides methods for loading resources, retrieving log category data, and
    /// saving log category data to a persistent storage. Implementations of this interface are responsible for ensuring
    /// that the data is stored and retrieved in a consistent and reliable manner.</remarks>
    internal interface ILogCategoryPersistence
    {
        /// <summary>
        /// Loads the necessary resources or data required for the operation.
        /// </summary>
        /// <remarks>This method should be called before attempting to use the associated functionality. 
        /// Ensure that any prerequisites are met before invoking this method.</remarks>
        void Load();

        /// <summary>
        /// Attempts to retrieve the saveable data associated with the specified category name.
        /// </summary>
        /// <remarks>This method does not throw an exception if the specified category name does not
        /// exist. Instead, it returns <see langword="false"/> and sets <paramref name="data"/> to its default
        /// value.</remarks>
        /// <param name="categoryName">The name of the category to retrieve data for. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="data">When this method returns, contains the saveable data associated with the specified category name, if the
        /// category exists; otherwise, the default value for the type of <paramref name="data"/>.</param>
        /// <returns><see langword="true"/> if the saveable data for the specified category name was successfully retrieved;
        /// otherwise, <see langword="false"/>.</returns>
        bool TryGet(string categoryName, out FLogCategorySaveableData data);

        /// <summary>
        /// Saves the specified log category data to a persistent storage.
        /// </summary>
        /// <param name="data">The log category data to be saved. Cannot be <see langword="null"/>.</param>
        void Save(FLogCategorySaveableData data);
    }
}
