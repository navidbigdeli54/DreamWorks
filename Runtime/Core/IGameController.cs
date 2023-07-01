/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public interface IGameController : IService
    {
        #region Property
        Action OnGameModeBegunPlay { get; set; }

        Action OnGameModeEnded { get; set; }

        IGameMode CurrentGameMode { get; }
        #endregion
    }
}