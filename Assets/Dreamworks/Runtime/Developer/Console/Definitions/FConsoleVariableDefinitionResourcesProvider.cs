using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Definitions
{
    /// <summary>
    /// Provides access to console variable definitions stored in a Resources asset.
    /// </summary>
    /// <remarks>This class retrieves console variable definitions from a predefined Resources asset located
    /// at <c>DreamWorks/DA_ConsoleVariableDefinitions</c>. If the asset is missing, an empty collection of definitions
    /// is returned, and a warning is logged the first time this occurs.</remarks>
    internal sealed class FConsoleVariableDefinitionResourcesProvider : IConsoleVariableDefinitionProvider
    {
        #region Fields
        private const string ResourcePath = "DreamWorks/DA_ConsoleVariableDefinitions";

        private readonly ILogProvider logProvider;

        private FConsoleVariableDefinitionRepository repository;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the collection of console variable definitions.
        /// </summary>
        public IReadOnlyList<FConsoleVariableDefinition> Definitions
        {
            get
            {
                if (repository == null)
                {
                    LoadRepository();
                }

                return repository == null ? System.Array.Empty<FConsoleVariableDefinition>() : repository.Definitions;
            }
        }
        #endregion

        #region Constructors
        public FConsoleVariableDefinitionResourcesProvider(ILogProvider logProvider)
        {
            this.logProvider = logProvider;
        }
        #endregion

        #region Private Methods
        private void LoadRepository()
        {
            repository = Resources.Load<FConsoleVariableDefinitionRepository>(ResourcePath);

            if (repository != null)
            {
                logProvider.LogVerbose($"Loaded console variable definitions from Resources/{ResourcePath} ({repository.Definitions.Count} definition(s)).", repository);

                return;
            }

            logProvider.LogWarning($"Console variable definition asset was not found at Resources/{ResourcePath}; no asset-defined variables will be registered.");
        }
        #endregion
    }
}
