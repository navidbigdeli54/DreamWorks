/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.Analytics
{
    public enum EProgressionStatus : byte
    {
        None = 0,
        Started = 1,
        Completed = 2,
        Failed = 3
    }

    public enum EResourceOperation : byte
    {
        None = 0,
        Gain = 1,
        Consume = 2
    }

    public interface IAnalyticsProvider : IService
    {
        void SendDesignEvent(string designEvent);

        void SendDesignEvent(string designEvent, float value);

        void SendProgressionEvent(string name, EProgressionStatus status);

        void SendProgressionEvent(string name, EProgressionStatus status, int value);

        void SendResouceEvent(EResourceOperation operation, string currency, float amount, string itemType, string itemId);

        void SendPurchaseEvent(string itemType, string itemId, string currency, int amount, string origin);
    }
}