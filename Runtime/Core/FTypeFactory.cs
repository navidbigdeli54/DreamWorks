/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Core
{
	public class FTypeFactory<TDerived, TType> : FSingleton<TDerived> where TDerived : class
	{
		#region Fields
		protected readonly Dictionary<string, FactoryMethod> registeredTypes = new Dictionary<string, FactoryMethod>();
		#endregion

		#region Methods
		public void RegisterType<Type>() where Type : TType, new()
		{
			FAssert.IsFalse(registeredTypes.ContainsKey(typeof(Type).Name), $"{GetType().Name}: {typeof(Type)} is already exist.");
			registeredTypes.Add(typeof(Type).Name, () => new Type());
		}

		public void RegisterType<Type>(string name) where Type : TType, new()
		{
			FAssert.IsFalse(registeredTypes.ContainsKey(name), $"{GetType().Name}: {name} is already exist.");
			registeredTypes.Add(name, () => new Type());
		}

		public TType Create(string typeName)
		{
			FAssert.IsFalse(string.IsNullOrEmpty(typeName), $"{GetType().Name}: Can't pass null or empty type name!");
			FAssert.IsTrue(registeredTypes.ContainsKey(typeName), $"{GetType().Name}: {typeName} does not exist.");

			return registeredTypes[typeName].Invoke();
		}
		#endregion

		#region Nested Type
		protected delegate TType FactoryMethod();
		#endregion
	}

}