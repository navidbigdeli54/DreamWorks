using DreamMachineGameStudio.DreamWorks.Core;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console
{
    /// <summary>
    /// Provides console commands for reading and updating registered console variables.
    /// </summary>
    public static class FConsoleVariableCommands
    {
        #region Public Methods
        /// <summary>
        /// Sets a console variable using `set &lt;variable_name&gt; &lt;value&gt;`.
        /// </summary>
        [AConsoleMethod("set", "Set a console variable: set <variable_name> <value>.")]
        public static string SetConsoleVariable(string variableName, params string[] valueParts)
        {
            if (string.IsNullOrWhiteSpace(variableName) || valueParts == null || valueParts.Length == 0)
            {
                return "Usage: set <variable_name> <value>.";
            }

            IDeveloperConsole developerConsole = GetDeveloperConsole();

            if (developerConsole == null || !developerConsole.TryGetVariable(variableName, out IConsoleVariable variable))
            {
                return $"Unknown console variable '{variableName}'.";
            }

            string value = string.Join(" ", valueParts);

            return variable.TrySetValue(value) ? $"\"{variable.Name}\" = \"{variable.GetValue()}\"" : $"Invalid value for console variable '{variableName}'.";
        }

        /// <summary>
        /// Reads a console variable using `read &lt;variable_name&gt;`.
        /// </summary>
        [AConsoleMethod("read", "Read a console variable: read <variable_name>.")]
        public static string ReadConsoleVariable(string variableName)
        {
            IDeveloperConsole developerConsole = GetDeveloperConsole();

            if (developerConsole == null || !developerConsole.TryGetVariable(variableName, out IConsoleVariable variable))
            {
                return $"Unknown console variable '{variableName}'.";
            }

            return $"\"{variable.Name}\" = \"{variable.GetValue()}\"";
        }
        #endregion

        #region Private Methods
        private static IDeveloperConsole GetDeveloperConsole()
        {
            return FDeveloperConsole.Instance;
        }
        #endregion
    }
}
