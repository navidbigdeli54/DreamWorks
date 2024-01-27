/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public abstract class FSingleton<TDerived> where TDerived : class
    {
		public static TDerived Instance { get; } = (TDerived)Activator.CreateInstance(typeof(TDerived), true);
	}
}