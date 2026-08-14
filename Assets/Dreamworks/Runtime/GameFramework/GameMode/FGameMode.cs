using System;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Extensions;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.GameFramework.HUD;
using DreamMachineGameStudio.DreamWorks.GameFramework.Pawn;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Log;
using DreamMachineGameStudio.DreamWorks.GameFramework.Controller;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework;

namespace DreamMachineGameStudio.DreamWorks.GameFramework.GameMode
{
    public class FGameMode : IGameMode
    {
        #region Properties
        protected IGameInstance GameInstance { get; }

        protected ILogProvider LogProvider { get; }

        protected IGameWorld GameWorld { get; private set; }

        protected FGameModeSettings GameModeSettings { get; private set; }
        #endregion

        #region Constructors
        public FGameMode(IGameInstance gameInstace, IGameWorld gameWorld, FGameModeSettings gameModeSettings, ILogProvider logProvider)
        {
            GameInstance = gameInstace;

            GameWorld = gameWorld;

            GameModeSettings = gameModeSettings;

            LogProvider = logProvider;
        }
        #endregion

        #region IGameMode Implementation
        async Task IGameMode.InitGameAsync(IGameWorld gameWorld, string sceneName)
        {
            LogProvider.Log($"InitGame: {sceneName}");

            GameWorld = gameWorld;

            try
            {
                await InitGameAsync(gameWorld, sceneName);
            }
            catch (Exception exception)
            {
                LogProvider.LogError(exception.Message);
            }
        }

        async Task IGameMode.StartPlayAsync()
        {
            LogProvider.Log("StartPlay");

            CreateLocalPlayer();

            try
            {
                await StartPlay();
            }
            catch (Exception exception)
            {
                LogProvider.LogError(exception.Message);
            }
        }

        void IGameMode.Tick(float deltaTime)
        {
            try
            {
                Tick(deltaTime);
            }
            catch (Exception exception)
            {
                LogProvider.LogError(exception.Message);
            }
        }

        async Task IGameMode.EndPlayAsync()
        {
            LogProvider.Log("EndPlay");

            try
            {
                await EndPlayAsync();
            }
            catch (Exception exception)
            {
                LogProvider.LogError(exception.Message);
            }
        }
        #endregion

        #region Protected Methods
        protected virtual Task InitGameAsync(IGameWorld gameWorld, string sceneName)
        {
            return Task.CompletedTask;
        }

        protected virtual Task StartPlay()
        {
            return Task.CompletedTask;
        }

        protected virtual void Tick(float deltaTime)
        {

        }

        protected virtual Task EndPlayAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual void CreateLocalPlayer()
        {
            CPlayerControllerComponent playerController = CreatePlayerController();
            if (playerController == null)
            {
                LogProvider.LogError("Could not create player controller!");

                return;
            }

            CHUDComponent hud = CreateHUDFor(playerController);
            if (hud == null)
            {
                LogProvider.LogError("Could not create player HUD!");

                return;
            }

            CPawnComponent pawn = CreateDefaultPawnFor(playerController);
            if (pawn == null)
            {
                LogProvider.LogError("Could not create player pawn!");

                return;
            }

            PossessPawnByPlayerController(playerController, pawn);
        }

        protected virtual CPlayerControllerComponent CreatePlayerController()
        {
            if (GameModeSettings.PlayerController == null)
            {
                LogProvider.LogWarning($"{GameModeSettings} has an invalid player controller!");

                return null;
            }

            return GameWorld.SpawnGameObject(GameModeSettings.PlayerController, Vector3.zero, Quaternion.identity);
        }

        protected virtual CHUDComponent CreateHUDFor(CPlayerControllerComponent playerController)
        {
            if (GameModeSettings.HUD == null)
            {
                LogProvider.LogWarning($"{GameModeSettings} has an invalid HUD!");

                return null;
            }

            CHUDComponent HUD = GameWorld.SpawnGameObject(GameModeSettings.HUD, Vector3.zero, Quaternion.identity);
            if (HUD != null)
            {
                playerController.SetHUD(HUD);
            }

            return HUD;
        }

        protected virtual CPawnComponent CreateDefaultPawnFor(CPlayerControllerComponent playerComponent)
        {
            if (GameModeSettings.Pawn == null)
            {
                LogProvider.LogWarning($"{GameModeSettings} has an invalid Pawn!");

                return null;
            }

            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;

            playerComponent.LastPlayerStart = FindPlayerStart();

            if (playerComponent.LastPlayerStart != null)
            {
                playerComponent.LastPlayerStart.transform.GetPositionAndRotation(out position, out rotation);
            }

            return GameWorld.SpawnGameObject(GameModeSettings.Pawn, position, rotation);
        }

        protected virtual CPlayerStartComponent FindPlayerStart()
        {
            IReadOnlyList<CPlayerStartComponent> existedPlayerStarts = GameWorld.FindComponents<CPlayerStartComponent>();

            IReadOnlyList<CPlayerControllerComponent> existedPlayerControllers = GameWorld.FindComponents<CPlayerControllerComponent>();

            for (int i = 0; i < existedPlayerStarts.Count; i++)
            {
                CPlayerStartComponent playerStart = existedPlayerStarts[i];

                if (playerStart.IsActive == false)
                {
                    continue;
                }

                if (existedPlayerControllers.Any(x => x.LastPlayerStart == playerStart) == false)
                {
                    return playerStart;
                }
            }

            return existedPlayerStarts.RandomRange();
        }

        protected virtual void PossessPawnByPlayerController(CPlayerControllerComponent playerController, CPawnComponent pawn)
        {
            playerController.Possess(pawn);
        }
        #endregion
    }
}