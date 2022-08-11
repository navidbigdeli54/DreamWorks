using System.Linq;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.Console
{
    public class FConsole : FSingleton<FConsole>
    {
        #region Fields
        private Dictionary<string, FCommand> _commands = new Dictionary<string, FCommand>();
        #endregion

        #region Public Methods
        public void AddCommand(FCommand command)
        {
            _commands.Add(command.Name.ToLower(), command);
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
