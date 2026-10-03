using System;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    public readonly struct FConsoleCommandOutputEntry
    {
        #region Properties
        public DateTime TimeStamp { get; }

        public string Message { get; }

        public EConsoleCommandOutputType MessageType { get; }
        #endregion

        #region Constructors
        public FConsoleCommandOutputEntry(string message, EConsoleCommandOutputType messageType)
        {
            TimeStamp = DateTime.Now;
            Message = message;
            MessageType = messageType;
        }
        #endregion
    }
}