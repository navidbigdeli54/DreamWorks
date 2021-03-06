/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/


namespace DreamMachineGameStudio.Dreamworks.Core
{
    public interface IGameManagement : IService
    {
        #region Property
        IGameMode CurrentGameMode { get; }
        #endregion
    }
}