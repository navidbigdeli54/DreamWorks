namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    public interface IConsoleObject
    {
        string Name { get; }

        string Description { get; }

        EConsoleObjectType ObjectType { get; }
    }
}
