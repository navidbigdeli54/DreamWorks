using System.Collections.Generic;
using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Definitions
{
    /// <summary>
    /// Loads the console-variable definition asset from the project's Resources folder.
    /// </summary>
    internal sealed class FConsoleVariableDefinitionResourcesProvider : IConsoleVariableDefinitionProvider
    {
        #region Fields
        private const string ResourcePath = "DreamWorks/DA_ConsoleVariableDefinitions";

        private FConsoleVariableDefinitionRepository repository;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the definitions from the Resources asset, or an empty collection when it is missing.
        /// </summary>
        public IReadOnlyList<FConsoleVariableDefinition> Definitions
        {
            get
            {
                if (repository == null)
                {
                    repository = Resources.Load<FConsoleVariableDefinitionRepository>(ResourcePath);
                }

                return repository == null ? System.Array.Empty<FConsoleVariableDefinition>() : repository.Definitions;
            }
        }
        #endregion
    }
}
