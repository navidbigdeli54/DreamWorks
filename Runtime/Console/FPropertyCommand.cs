using System.Reflection;
using System.ComponentModel;

namespace DreamMachineGameStudio.Dreamworks.Console
{
    public class FPropertyCommand : FCommand
    {
        #region Fields
        private PropertyInfo _propertyInfo;

        private TypeConverter _typeConverter;
        #endregion

        #region Constructors
        public FPropertyCommand(string name, object target, PropertyInfo propertyInfo) : base(name, target)
        {
            _propertyInfo = propertyInfo;

            _typeConverter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
        }
        #endregion

        #region Public Methods
        public override void Execute(string[] parameters)
        {
            object param = _typeConverter.ConvertFrom(parameters[0]);

            _propertyInfo.SetValue(Target, param);
        }
        #endregion
    }
}
