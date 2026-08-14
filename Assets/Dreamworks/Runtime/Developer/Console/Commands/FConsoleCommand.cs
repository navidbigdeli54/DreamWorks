using System;
using System.Reflection;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Commands
{
    public sealed class FConsoleCommand : IConsoleCommand
    {
        #region Properties
        public string Name { get; }

        public string Description { get; }

        public object Object { get; }

        public MethodInfo MethodInfo { get; }

        public EConsoleObjectType ObjectType => EConsoleObjectType.Command;
        #endregion

        #region Constructors
        public FConsoleCommand(string name, string description, MethodInfo methodInfo)
        {
            Name = name;
            Description = description;
            MethodInfo = methodInfo;
        }

        public FConsoleCommand(string name, string description, object obj, MethodInfo method)
            : this(name, description, method)
        {
            Object = obj;
        }
        #endregion

        #region Public Methods
        public object Execute(string[] arguments)
        {
            ParameterInfo[] parameters = MethodInfo.GetParameters();

            object[] values = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; ++i)
            {
                values[i] = ConvertArgument(arguments[i], parameters[i].ParameterType);
            }

            return MethodInfo.Invoke(Object, values);
        }
        #endregion

        #region Private Methods
        private static object ConvertArgument(string value, Type type)
        {
            if (type == typeof(string))
                return value;

            if (type == typeof(bool))
                return bool.Parse(value);

            if (type == typeof(int))
                return int.Parse(value);

            if (type == typeof(float))
                return float.Parse(value);

            if (type.IsEnum)
                return Enum.Parse(type, value, true);

            return Convert.ChangeType(value, type);
        }
        #endregion
    }
}