namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions
{
    /// <summary>
    /// Represents a suggestion for a console command, including its name and description.
    /// </summary>
    /// <remarks>This struct is typically used to provide autocomplete or help functionality for console
    /// commands.</remarks>
    public readonly struct FConsoleCommandSuggestion
    {
        #region Fields
        /// <summary>
        /// Gets the name associated with the command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the command.
        /// </summary>
        public string Description { get; }
        #endregion

        #region Constructors
        public FConsoleCommandSuggestion(string name, string description)
        {
            Name = name;
            Description = description;
        } 
        #endregion
    }
}