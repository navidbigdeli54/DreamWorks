using System;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console
{
    public readonly struct FConsoleOutputEntry
    {
        public DateTime TimeStamp { get; }

        public string Message { get; }

        public EConsoleOutputType MessageType { get; }

        public FConsoleOutputEntry(string message, EConsoleOutputType messageType)
        {
            TimeStamp = DateTime.Now;
            Message = message;
            MessageType = messageType;
        }
    }
}