using System;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions
{
    /// <summary>
    /// Represents a single entry in the output of a console command, including a timestamp, message, and message type.
    /// </summary>
    /// <remarks>This structure is used to encapsulate the details of a console command's output, such as when
    /// the message was generated, the content of the message, and its associated type (e.g., informational, warning, or error).</remarks>
    internal readonly struct FConsoleCommandOutputEntry
    {
        #region Properties
        internal DateTimeOffset TimeStamp { get; }

        internal string Message { get; }

        internal EConsoleCommandOutputType MessageType { get; }
        #endregion

        #region Constructors
        internal FConsoleCommandOutputEntry(string message, EConsoleCommandOutputType messageType)
        {
            TimeStamp = DateTimeOffset.UtcNow;
            Message = message;
            MessageType = messageType;
        }
        #endregion
    }
}