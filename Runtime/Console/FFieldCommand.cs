using System.Reflection;

namespace DreamMachineGameStudio.Dreamworks
{
    public class FFieldCommand : FCommand
    {
        #region Properties
        public FieldInfo FieldInfo { get; }
        #endregion

        #region Constructors
        public FFieldCommand(string name, FieldInfo fieldInfo) : base(name)
        {
            FieldInfo = fieldInfo;
        }
        #endregion
    }
}
