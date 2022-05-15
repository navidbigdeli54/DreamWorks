/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.Serialization.Json
{
    public class FJsonString : FJsonNode
    {
        #region Fields
        private string _value;
        #endregion

        #region Constructors
        public FJsonString(string s)
        {
            _value = s;
        }
        #endregion

        #region Operator Overloads
        public static implicit operator string(FJsonString jsonString) => jsonString._value;
        #endregion

        #region Public Methods
        public override string ToString() => ToString();

        public override string ToString(int intentLevel = 0) => $"\"{Escape(_value)}\"";
        #endregion
    }
}