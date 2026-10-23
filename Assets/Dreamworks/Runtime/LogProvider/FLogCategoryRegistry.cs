using System;
using System.Text;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.LogProvider.Definitions;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction.Persistence;
using DreamMachineGameStudio.DreamWorks.LogProvider.Definitions.Persitence;

namespace DreamMachineGameStudio.DreamWorks.LogProvider
{
    /// <summary>
    /// Registers log categories, resolves persisted settings, and exposes runtime controls.
    /// </summary>
    public static class FLogCategoryRegistry
    {
        #region Fields
        private static readonly Dictionary<string, FLogCategoryRuntimeSettings> categories = new Dictionary<string, FLogCategoryRuntimeSettings>(StringComparer.OrdinalIgnoreCase);

        private static ILogCategoryPersistence persistence;

        private static int globalVerbosity = (int)ELogVerbosity.Log;
        #endregion

        #region Properties
        public static ELogVerbosity GlobalVerbosity => (ELogVerbosity)System.Threading.Volatile.Read(ref globalVerbosity);
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads saved category overrides and applies them to categories already registered.
        /// </summary>
        internal static void Initialize(ILogCategoryPersistence categoryPersistence)
        {
            if (categoryPersistence == null)
            {
                throw new ArgumentNullException(nameof(categoryPersistence));
            }

            categoryPersistence.Load();

            persistence = categoryPersistence;

            foreach (FLogCategoryRuntimeSettings settings in categories.Values)
            {
                ApplyPersistedSettings(settings);
            }
        }

        /// <summary>
        /// Releases the active persistence service while preserving registered category definitions.
        /// </summary>
        public static void Shutdown()
        {
            persistence = null;
        }

        /// <summary>
        /// Returns a snapshot of the registered category names.
        /// </summary>
        public static string[] GetRegisteredCategoryNames()
        {
            string[] names = new string[categories.Count];
            int index = 0;
            foreach (FLogCategoryRuntimeSettings settings in categories.Values)
            {
                names[index++] = settings.Name;
            }

            Array.Sort(names, StringComparer.OrdinalIgnoreCase);
            return names;
        }

        /// <summary>
        /// Attempts to retrieve the effective runtime settings for a registered category.
        /// </summary>
        public static bool TryGetCategorySettings(string categoryName, out bool isEnabled, out ELogVerbosity verbosityThreshold)
        {
            isEnabled = false;
            verbosityThreshold = ELogVerbosity.NoLogging;

            if (string.IsNullOrWhiteSpace(categoryName) || !categories.TryGetValue(categoryName, out FLogCategoryRuntimeSettings settings))
            {
                return false;
            }

            isEnabled = settings.IsEnabled;
            verbosityThreshold = settings.VerbosityThreshold;
            return true;
        }

        /// <summary>
        /// Attempts to retrieve the effective runtime settings for a category by name.
        /// </summary>
        internal static bool TryGetCategory(string categoryName, out FLogCategoryRuntimeSettings settings)
        {
            return categories.TryGetValue(categoryName, out settings);
        }

        /// <summary>
        /// Determines whether a category message passes global and per-category filters.
        /// </summary>
        internal static bool IsEnabled(FLogCategoryRuntimeSettings settings, ELogVerbosity verbosity)
        {
            if ((byte)verbosity > (byte)ELogVerbosity.VeryVerbose)
            {
                return false;
            }

            ELogVerbosity globalThreshold = GlobalVerbosity;
            ELogVerbosity categoryThreshold = settings.VerbosityThreshold;
            return settings.IsEnabled
                && globalThreshold != ELogVerbosity.NoLogging
                && categoryThreshold != ELogVerbosity.NoLogging
                && verbosity <= globalThreshold
                && verbosity <= categoryThreshold;
        }
        #endregion

        #region Internal Methods
        internal static FLogCategoryRuntimeSettings RegisterCategory(FLogCategory category)
        {
            if (categories.TryGetValue(category.Name, out FLogCategoryRuntimeSettings existing))
            {
                if (existing.DefaultVerbosity != category.DefaultVerbosity)
                {
                    throw new InvalidOperationException($"Log category '{category.Name}' was registered with conflicting default verbosity values.");
                }

                return existing;
            }

            FLogCategoryRuntimeSettings settings = new FLogCategoryRuntimeSettings(category.Name, true, category.DefaultVerbosity);
            categories.Add(category.Name, settings);

            ApplyPersistedSettings(settings);

            return settings;
        }
        #endregion

        #region Console Methods
        [AConsoleMethod("logverbosity", "Set the global log verbosity threshold.")]
        public static void SetGlobalVerbosity(ELogVerbosity verbosity)
        {
            if (!Enum.IsDefined(typeof(ELogVerbosity), verbosity))
            {
                throw new ArgumentOutOfRangeException(nameof(verbosity), "Specify a valid log verbosity value.");
            }

            System.Threading.Volatile.Write(ref globalVerbosity, (int)verbosity);
        }

        [AConsoleMethod("logcategories", "List registered log categories and their current settings.")]
        public static string ListCategories()
        {
            StringBuilder output = new StringBuilder();

            foreach (FLogCategoryRuntimeSettings settings in categories.Values)
            {
                if (output.Length > 0)
                {
                    output.AppendLine();
                }

                output.Append(settings.Name)
                    .Append(" Enabled=").Append(settings.IsEnabled)
                    .Append(" Verbosity=").Append(settings.VerbosityThreshold);
            }

            return output.Length == 0 ? "No log categories have been registered." : output.ToString();
        }

        [AConsoleMethod("logcategoryenable", "Enable or disable a log category: logcategoryenable <category> <true|false>.")]
        public static void SetCategoryEnabled(string categoryName, bool isEnabled)
        {
            if (!categories.TryGetValue(categoryName, out FLogCategoryRuntimeSettings settings))
            {
                throw new ArgumentException($"Log category '{categoryName}' is not registered.", nameof(categoryName));
            }

            settings.SetEnabled(isEnabled);

            SaveSettings(settings);
        }

        [AConsoleMethod("logcategoryverbosity", "Set a category threshold: logcategoryverbosity <category> <verbosity>.")]
        public static void SetCategoryVerbosity(string categoryName, ELogVerbosity verbosityThreshold)
        {
            if (!Enum.IsDefined(typeof(ELogVerbosity), verbosityThreshold))
            {
                throw new ArgumentOutOfRangeException(nameof(verbosityThreshold), "Specify a valid log verbosity value.");
            }

            if (!categories.TryGetValue(categoryName, out FLogCategoryRuntimeSettings settings))
            {
                throw new ArgumentException($"Log category '{categoryName}' is not registered.", nameof(categoryName));
            }

            settings.SetVerbosityThreshold(verbosityThreshold);

            SaveSettings(settings);
        }
        #endregion

        #region Private Methods
        private static void ApplyPersistedSettings(FLogCategoryRuntimeSettings settings)
        {
            if (persistence != null && persistence.TryGet(settings.Name, out FLogCategorySaveableData record))
            {
                settings.Apply(record.IsEnabled, record.VerbosityThreshold);
            }
        }

        private static void SaveSettings(FLogCategoryRuntimeSettings settings)
        {
            persistence?.Save(new FLogCategorySaveableData(settings.Name, settings.IsEnabled, settings.VerbosityThreshold));
        }
        #endregion
    }
}
