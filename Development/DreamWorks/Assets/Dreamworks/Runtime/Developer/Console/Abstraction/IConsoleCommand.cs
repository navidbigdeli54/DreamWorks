namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    public interface IConsoleCommand : IConsoleObject
    {
        object Execute(string[] arguments);
    }
}
