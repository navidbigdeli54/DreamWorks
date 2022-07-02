/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

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

        #region Operator Overloads
        public static implicit operator double(FJsonNumber jsonNumber) => jsonNumber._value;

        public static implicit operator int(FJsonNumber jsonNumber) => (int)jsonNumber._value;
        #endregion

        #region Public Methods
        public override string ToString() => ToString();

        public override string ToString(int intentLevel = 0) => $"{_value}";
        #endregion
    }
}