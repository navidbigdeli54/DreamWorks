/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable CS0649

using UnityEngine;
using UnityEngine.Serialization;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public class CLevelConfiguration : CComponent
    {
        #region Fields
        [SerializeField]
        [FSubclassFilter(typeof(FGameMode))]
        [FormerlySerializedAs("gameMode")]
        private FSubclass _gameMode;
        #endregion

        #region Properties
        public FSubclass GameMode => _gameMode;
        #endregion
    }
}