/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.Serialization.Json
{
    public class FJsonNull : FJsonNode
    {
        #region Fields
        public static readonly FJsonNull NULL = new FJsonNull();

        private const string NULL_STRING = "null";
        #endregion

        #region Public Methods
        public override string ToString() => ToString();

        public override string ToString(int intentLevel = 0) => NULL_STRING;
        #endregion
    }
}