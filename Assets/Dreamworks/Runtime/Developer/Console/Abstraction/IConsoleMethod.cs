namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    public interface IConsoleMethod : IConsoleObject
    {
        object Execute(string[] arguments);
    }
}
