/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

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

        #region Public Methods
        public override string ToString() => ToString();

        public override string ToString(int intentLevel = 0) => $"\"{Escape(_value)}\"";
        #endregion
    }
}