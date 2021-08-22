/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable IDE0044
#pragma warning disable CS0649

using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    [FScriptableObjectWizardAttribute("Dreamworks Configuration")]
    public class SDreamworksConfiguration : SScriptableObject
    {
        #region Field
        [SerializeField]
        [FormerlySerializedAs("dontLoadFramework")]
        private bool _dontLoadFramework = false;

        [SerializeField]
        [FSubclassFilter(typeof(FStartup))]
        [FormerlySerializedAs("startupClass")]
        private FSubclass _startupClass;
        #endregion

        #region Property
        public bool DontLoadFramework => _dontLoadFramework;

        public Type StartupClass => _startupClass.Type;
        #endregion
    }
}