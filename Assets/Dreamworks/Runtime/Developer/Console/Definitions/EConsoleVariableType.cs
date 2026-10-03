namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Definitions
{
    /// <summary>Defines supported value types for user-authored console variables.</summary>
    public enum EConsoleVariableType : byte
    {
        /// <summary>Indicates that a definition has no valid type selected.</summary>
        None = 0,

        /// <summary>Represents a Boolean value.</summary>
        Boolean = 1,

        /// <summary>Represents a string value.</summary>
        String = 2,

        /// <summary>Represents a single-precision floating-point value.</summary>
        Float = 3,

        /// <summary>Represents a 32-bit integer value.</summary>
        Integer = 4
    }
}
