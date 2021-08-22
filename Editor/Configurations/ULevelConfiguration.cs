/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using UnityEditor;
using UnityEngine;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.Editor
{
    public class ULevelConfiguration
    {
        [MenuItem(itemName: "DreamMachineGameStudio/Level Configuration", priority = 1)]
        private static void OpenConfiguration()
        {
            CLevelConfiguration configuration = Object.FindObjectOfType<CLevelConfiguration>();

            if (configuration == null)
            {
                configuration = new GameObject(nameof(CLevelConfiguration)).AddComponent<CLevelConfiguration>();
            }

            Selection.activeObject = configuration;
        }
    }
}