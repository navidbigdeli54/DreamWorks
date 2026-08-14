using DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console
{
    public readonly struct FConsoleSuggestion
    {
        #region Fields
        public string Name { get; }

        public string Description { get; }
        #endregion

        #region Constructors
        public FConsoleSuggestion(string name, string description)
        {
            Name = name;
            Description = description;
        } 
        #endregion
    }
}