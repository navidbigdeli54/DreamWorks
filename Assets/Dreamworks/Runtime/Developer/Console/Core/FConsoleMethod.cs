using System;
using System.Reflection;
using System.Globalization;
using System.Linq;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Core
{
    public sealed class FConsoleMethod : IConsoleMethod
    {
        #region Properties
        public string Name { get; }

        public string Description { get; }

        public object Object { get; }

        public MethodInfo MethodInfo { get; }

        public EConsoleObjectType ObjectType => EConsoleObjectType.Method;
        #endregion

        #region Constructors
        public FConsoleMethod(string name, string description, MethodInfo methodInfo)
        {
            Name = name;

            Description = description;

            MethodInfo = methodInfo;
        }

        public FConsoleMethod(string name, string description, object obj, MethodInfo method)
            : this(name, description, method)
        {
            Object = obj;
        }
        #endregion

        #region Public Methods
        public object Execute(string[] arguments)
        {
            ParameterInfo[] parameterInfos = MethodInfo.GetParameters();
            int requiredParameterCount = parameterInfos.Count(parameterInfo => !parameterInfo.IsOptional && !Attribute.IsDefined(parameterInfo, typeof(ParamArrayAttribute)));
            bool hasParameterArray = parameterInfos.Length > 0 && Attribute.IsDefined(parameterInfos[^1], typeof(ParamArrayAttribute));
            int maximumParameterCount = hasParameterArray ? int.MaxValue : parameterInfos.Length;

            if (arguments.Length < requiredParameterCount || arguments.Length > maximumParameterCount)
            {
                string expectedArgumentCount = requiredParameterCount == maximumParameterCount
                    ? requiredParameterCount.ToString(CultureInfo.InvariantCulture)
                    : $"{requiredParameterCount} to {(hasParameterArray ? "any number of" : maximumParameterCount.ToString(CultureInfo.InvariantCulture))}";
                throw new ArgumentException($"Command '{Name}' expects {expectedArgumentCount} arguments but received {arguments.Length}.");
            }

            object[] parameterValues = new object[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; ++i)
            {
                if (Attribute.IsDefined(parameterInfos[i], typeof(ParamArrayAttribute)))
                {
                    Type elementType = parameterInfos[i].ParameterType.GetElementType();
                    int parameterArrayLength = arguments.Length - i;
                    Array parameterArray = Array.CreateInstance(elementType, parameterArrayLength);
                    for (int argumentIndex = 0; argumentIndex < parameterArrayLength; ++argumentIndex)
                    {
                        parameterArray.SetValue(ConvertParameter(arguments[i + argumentIndex], elementType), argumentIndex);
                    }

                    parameterValues[i] = parameterArray;
                    break;
                }

                parameterValues[i] = i < arguments.Length
                    ? ConvertParameter(arguments[i], parameterInfos[i].ParameterType)
                    : parameterInfos[i].DefaultValue;
            }

            return MethodInfo.Invoke(Object, parameterValues);
        }
        #endregion

        #region Private Methods
        private object ConvertParameter(string value, Type type)
        {
            Type conversionType = Nullable.GetUnderlyingType(type) ?? type;

            if (conversionType == typeof(string))
            {
                return value;
            }

            if (conversionType.IsEnum)
            {
                return Enum.Parse(conversionType, value, true);
            }

            return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
        }
        #endregion
    }
}