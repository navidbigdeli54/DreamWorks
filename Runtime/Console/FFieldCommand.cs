using System.Reflection;
using System.ComponentModel;

namespace DreamMachineGameStudio.Dreamworks.Console
{
    public class FFieldCommand : FCommand
    {
        #region Fields
        public readonly FieldInfo _fieldInfo;

        private readonly TypeConverter _converter;
        #endregion

        #region Constructors
        public FFieldCommand(string name, object target, FieldInfo fieldInfo) : base(name, target)
        {
            _fieldInfo = fieldInfo;

            _converter = TypeDescriptor.GetConverter(fieldInfo.FieldType);

        }
        #endregion

        #region Public Methods
        public sealed override void Execute(string[] parameters)
        {
            object param = _converter.ConvertFrom(parameters[0]);

            _fieldInfo.SetValue(Target, param);
        }
        #endregion
    }
}
