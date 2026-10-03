using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Assets;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Definitions
{
    /// <summary>
    /// Stores authored console-variable definitions as a project data asset.
    /// </summary>
    [CreateAssetMenu(fileName = "ConsoleVariableDefinitions", menuName = "DreamWorks/Console/Variable Definition Repository")]
    public sealed class FConsoleVariableDefinitionRepository : UDataAsset, IConsoleVariableDefinitionProvider
    {
        #region Fields
        [SerializeField]
        private List<FConsoleVariableDefinition> definitions = new();
        #endregion

        #region Properties
        /// <summary>
        /// Gets the configured variable definitions.
        /// </summary>
        public IReadOnlyList<FConsoleVariableDefinition> Definitions => definitions;
        #endregion

        #region IConsoleVariableDefinitionProvider Implementation
        IReadOnlyList<FConsoleVariableDefinition> IConsoleVariableDefinitionProvider.Definitions => Definitions;
        #endregion
    }
}
