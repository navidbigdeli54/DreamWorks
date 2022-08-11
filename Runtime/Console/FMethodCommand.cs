using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.Console
{
    public class FMethodCommand : FCommand
    {
        #region Fields
        private MethodInfo _methodInfo;

        private List<TypeConverter> _typeConverters = new List<TypeConverter>();
        #endregion

        #region Constructors
        public FMethodCommand(string name, object target, MethodInfo methodInfo) : base(name, target)
        {
            _methodInfo = methodInfo;

            ParameterInfo[] parameterInfos = _methodInfo.GetParameters();
            foreach (ParameterInfo parameterInfo in parameterInfos)
            {
                Type type = parameterInfo.ParameterType;
                _typeConverters.Add(TypeDescriptor.GetConverter(type));
            }
        }
        #endregion

        #region Public Methods
        public override void Execute(string[] parameters)
        {
            object[] parsedParameters = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; ++i)
            {
                parsedParameters[i] = _typeConverters[i].ConvertFromString(parameters[i]);
            }

            _methodInfo.Invoke(Target, parsedParameters);
        }
        #endregion
    }
}
