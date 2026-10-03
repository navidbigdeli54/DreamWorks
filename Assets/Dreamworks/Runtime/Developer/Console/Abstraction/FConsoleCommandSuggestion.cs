using DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    public readonly struct FConsoleCommandSuggestion
    {
        #region Fields
        public string Name { get; }

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