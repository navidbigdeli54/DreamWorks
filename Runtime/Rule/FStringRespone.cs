﻿/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.Rule
{
    public class FStringRespone : IResponse
    {
        #region Fields
        public readonly string Value;
        #endregion

        #region Constructor
        public FStringRespone(string response) => Value = response;
        #endregion
    }
}