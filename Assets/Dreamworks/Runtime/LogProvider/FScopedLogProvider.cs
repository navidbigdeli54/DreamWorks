using System;
using UnityEngine;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.LogProvider.Definitions;

namespace DreamMachineGameStudio.DreamWorks.LogProvider
{
    /// <summary>
    /// Provides scoped logging functionality for a specific log category.
    /// </summary>
    /// <remarks>This class implements the <see cref="ILogProvider"/> interface and enables logging messages
    /// with a specified verbosity level for a given <see cref="FLogCategory"/>. It ensures that messages are formatted
    /// and routed appropriately based on the category's runtime settings.</remarks>
    public class FScopedLogProvider : ILogProvider_Internal
    {
        #region Properties
        public FLogCategory Category { get; }
        #endregion

        #region Constructors
        public FScopedLogProvider(FLogCategory category)
        {
            Category = category ?? throw new ArgumentNullException(nameof(category));
        }
        #endregion

        #region ILogProvider Implementation
        bool ILogProvider.IsEnable(ELogVerbosity verbosity)
        {
            return IsEnable(verbosity);
        }

        void ILogProvider_Internal.Log(ELogVerbosity verbosity, string message, UnityEngine.Object context)
        {
            if (!IsEnable(verbosity))
            {
                return;
            }

            Write(Format(message, verbosity), verbosity, context);
        }

        void ILogProvider_Internal.Log(ELogVerbosity verbosity, Exception exception, string message, UnityEngine.Object context)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (!IsEnable(verbosity))
            {
                return;
            }

            string combinedMessage = string.IsNullOrEmpty(message) ? exception.ToString() : $"{message}{Environment.NewLine}{exception}";

            Write(Format(combinedMessage, verbosity), verbosity, context);
        }
        #endregion

        #region Private Methods
        private bool IsEnable(ELogVerbosity verbosity)
        {
            if (FLogCategoryRegistry.GlobalVerbositry < verbosity)
            {
                return false;
            }

            if (Category.RuntimeSettings.DefaultVerbositry < verbosity)
            {
                return false;
            }

            return Category.RuntimeSettings.IsEnabled;
        }

        private string Format(string message, ELogVerbosity verbosity)
        {
            return $"<b><color=#{ColorUtility.ToHtmlStringRGBA(Category.Color)}>[{Category.Name}] [{verbosity}]</color></b> {message}";
        }

        private static void Write(string message, ELogVerbosity verbosity, UnityEngine.Object context)
        {
            switch (verbosity)
            {
                case ELogVerbosity.Fatal:
                case ELogVerbosity.Error:
                    Debug.LogError(message, context);
                    break;
                case ELogVerbosity.Warning:
                    Debug.LogWarning(message, context);
                    break;
                default:
                    Debug.Log(message, context);
                    break;
            }
        }
        #endregion
    }
}
