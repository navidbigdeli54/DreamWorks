namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions
{
    /// <summary>
    /// Specifies the type of a console command, such as a method or variable.
    /// </summary>
    /// <remarks>This enumeration is used to categorize console command based on their type.  It includes
    /// values for methods, variables, and an undefined state.</remarks>
    public enum EConsoleCommandType : byte
    {
        /// <summary>
        /// Represents the absence of any specific value or option.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents the method type.
        /// </summary>
        Method = 1,

        /// <summary>
        /// Represents a variable type.
        /// </summary>
        Variable = 2
    }
}
