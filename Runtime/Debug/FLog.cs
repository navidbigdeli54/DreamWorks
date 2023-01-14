/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Diagnostics;

using UDebug = UnityEngine.Debug;

namespace DreamMachineGameStudio.Dreamworks.Debug
{
    public static class FLog
    {
        #region Method
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Info(string category, object message)
        {
            UDebug.Log($"<color=lime>{DateTime.Now} | {category} | {message}</color>");
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Warning(string category, object message)
        {
            UDebug.LogWarning($"<color=yellow>{DateTime.Now} | {category} | {message}</color>");
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Error(string category, object message)
        {
            UDebug.LogError($"<color=red>{DateTime.Now} | {category} | {message}</color>");
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Exception(string category, Exception exception)
        {
            UDebug.LogError($"<color=red>{DateTime.Now} | {category} | {exception}</color>");
        }
        #endregion
    }
}