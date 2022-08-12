using System.Linq;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.Console
{
    public class FConsole : FSingleton<FConsole>
    {
        #region Fields
        private static Dictionary<string, FCommand> _commands = new Dictionary<string, FCommand>();

        private static FDefaultCommands defualtCommands;
        #endregion

        #region Constructors
        static FConsole()
        {
            defualtCommands = new FDefaultCommands();
            FConsoleUtility.TryAddCommands(defualtCommands);
        }
        #endregion

        #region Public Methods
        public void AddCommand(FCommand command)
        {
            _commands.Add(command.Name.ToLower(), command);
        }

        public void RemoveCommand(FCommand command)
        {
            _commands.Remove(command.Name.ToLower());
        }

        public void ExecuteCommand(string commandString)
        {
            string[] splittedCommandString = commandString.Split(' ');

            string commandName = splittedCommandString[0];
            if (_commands.TryGetValue(commandName.ToLower(), out FCommand command))
            {
                command.Execute(splittedCommandString.TakeLast(splittedCommandString.Length - 1).ToArray());
            }
        }
        #endregion
    }
}
