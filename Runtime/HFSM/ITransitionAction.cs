﻿/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public interface ITransitionAction
    {
        #region Methods
        void Perform(FHFSM machine);
        #endregion
    }
}