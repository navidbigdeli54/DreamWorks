/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.Analytics
{
    public enum EProgressionStatus : byte
    {
        Started,
        Completed,
        Failed
    }

    public interface IAnalytics : IService
    {
        void SendProgressionEvent(string name, EProgressionStatus status);

        void SendProgressionEvent(string name, EProgressionStatus status, int value);
    }
}