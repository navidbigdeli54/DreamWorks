/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.Serialization.Json
{
    public class FJsonBool : FJsonNode
    {
        #region Fields
        private const string TRUE_STRING = "true";
        private const string FALSE_STRING = "false";

        private bool _value;
        #endregion

        #region Constructors
        public FJsonBool(bool b)
        {
            _value = b;
        }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return ToString();
        }

        public override string ToString(int intentLevel = 0) => _value ? TRUE_STRING : FALSE_STRING;
        #endregion
    }
}