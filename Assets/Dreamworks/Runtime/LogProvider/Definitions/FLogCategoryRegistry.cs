using System;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes;

namespace DreamMachineGameStudio.DreamWorks.LogProvider.Definitions
{
    /// <summary>
    /// Tracks categories and binds their runtime controls to the developer console.
    /// </summary>
    public static class FLogCategoryRegistry
    {
        #region Fields
        private static readonly Dictionary<string, FLogCategoryRuntimeSettings> categories = new(StringComparer.OrdinalIgnoreCase);
        #endregion

        #region Properties
        public static ELogVerbosity GlobalVerbositry { get; private set; } = ELogVerbosity.Log;
        #endregion

        #region Internal Methods
        internal static FLogCategoryRuntimeSettings RegisterCategory(FLogCategory category)
        {
            if (categories.TryGetValue(category.Name, out FLogCategoryRuntimeSettings existing))
            {
                if (existing.DefaultVerbositry != category.DefaultVerbosity)
                {
                    throw new InvalidOperationException($"Log category '{category.Name}' was registered with conflicting default verbosity values.");
                }

                return existing;
            }

            FLogCategoryRuntimeSettings settings = new(category.Name, true, category.DefaultVerbosity);

            categories.Add(category.Name, settings);

            return settings;
        }
        #endregion

        #region Private Methods
        [AConsoleMethod("logverbosity", "Set Global Log Verbositry, 0: No Logging, 7: VeryVerbose, 6:Verbose, 5: Log, 4: Display, 3: Warning, 2: Error, 1: Fatal")]
        private static void SetLogVerbosity(int verbosity)
        {
            GlobalVerbositry = (ELogVerbosity)verbosity;
        }
        #endregion
    }
}
