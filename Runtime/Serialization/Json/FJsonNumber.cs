/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.Serialization.Json
{
    public class FJsonNumber : FJsonNode
    {
        #region Fields
        private double _value;
        #endregion

        #region Constructors
        public FJsonNumber(double d)
        {
            _value = d;
        }
        #endregion

        #region Public Methods
        public override string ToString() => ToString();

        public override string ToString(int intentLevel = 0) => $"{_value}";
        #endregion
    }
}