using System.Reflection;
using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.Console
{
    public static class FConsoleUtility
    {
        #region Public Methods
        public static void TryAddCommands(object obj)
        {
            IReadOnlyList<FCommand> commands = FindCommands(obj);
            foreach (FCommand command in commands)
            {
                FConsole.Instance.AddCommand(command);
            }
        }

        public static void TryRemoveCommands(object obj)
        {
            IReadOnlyList<FCommand> commands = FindCommands(obj);
            foreach (FCommand command in commands)
            {
                FConsole.Instance.RemoveCommand(command);
            }
        }
        #endregion

        #region Private Methods
        private static IReadOnlyList<FCommand> FindCommands(object obj)
        {
            List<FCommand> commands = new List<FCommand>();

            commands.AddRange(GetFieldCommands(obj));
            commands.AddRange(GetPropertyCommands(obj));
            commands.AddRange(GetMethodCommands(obj));

            return commands;
        }

        private static IReadOnlyList<FCommand> GetFieldCommands(object obj)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            List<FCommand> commands = new List<FCommand>();

            FieldInfo[] fieldInfos = obj.GetType().GetFields(bindingFlags);
            for (int i = 0; i < fieldInfos.Length; ++i)
            {
                FieldInfo info = fieldInfos[i];
                if (info.IsDefined(typeof(FCommandAttribute)))
                {
                    FCommandAttribute attribute = info.GetCustomAttribute<FCommandAttribute>();

                    FFieldCommand fieldCommand = new FFieldCommand(attribute.Name, obj, info);

                    commands.Add(fieldCommand);
                }
            }

            return commands;
        }

        private static IReadOnlyList<FCommand> GetPropertyCommands(object obj)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            List<FCommand> commands = new List<FCommand>();

            PropertyInfo[] propertyInfos = obj.GetType().GetProperties(bindingFlags);
            for (int i = 0; i < propertyInfos.Length; ++i)
            {
                PropertyInfo info = propertyInfos[i];
                if (info.IsDefined(typeof(FCommandAttribute)))
                {
                    FCommandAttribute attribute = info.GetCustomAttribute<FCommandAttribute>();

                    FPropertyCommand propertyCommand = new FPropertyCommand(attribute.Name, obj, info);

                    commands.Add(propertyCommand);
                }
            }

            return commands;
        }

        private static IReadOnlyList<FCommand> GetMethodCommands(object obj)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            List<FCommand> commands = new List<FCommand>();

            MethodInfo[] methodInfos = obj.GetType().GetMethods(bindingFlags);
            for (int i = 0; i < methodInfos.Length; ++i)
            {
                MethodInfo info = methodInfos[i];
                if (info.IsDefined(typeof(FCommandAttribute)))
                {
                    FCommandAttribute attribute = info.GetCustomAttribute<FCommandAttribute>();

                    FMethodCommand propertyCommand = new FMethodCommand(attribute.Name, obj, info);

                    commands.Add(propertyCommand);
                }
            }

            return commands;
        }
        #endregion
    }
}
