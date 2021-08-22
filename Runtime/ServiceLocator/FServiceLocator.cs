/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.ServiceLocator
{
    public static class FServiceLocator
    {
        #region Fields
        public readonly static Type CLASS_TYPE = typeof(FServiceLocator);

        private readonly static IDictionary<Type, object> _registeredServices = new Dictionary<Type, object>();
        #endregion

        #region Properties
        public static IReadOnlyDictionary<Type, object> RegisteredServices => _registeredServices as IReadOnlyDictionary<Type, object>;
        #endregion

        #region Methods
        /// <summary>
        /// Register a service with given instance as singleton in ServiceLocator.
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <param name="service">Service instance</param>
        public static void Register<TService>(object service) where TService : class
        {
            FAssert.IsNotNull(service, "Service instance can't be null.");

            FAssert.IsNotNull(service as TService, $"`{service.GetType().Name}` Service instance doesn't implemented `{typeof(TService).Name}` interface.");

            Type serviceType = typeof(TService);

            FAssert.IsFalse(_registeredServices.ContainsKey(serviceType), $"`{service.GetType().Name}` service is already registered.");

            _registeredServices.Add(serviceType, service);

            FLog.Info(CLASS_TYPE.Name, $"`{serviceType.Name}` service has been registered. Service instance is `{service.GetType().Name}`.");
        }

        /// <summary>
        /// return instance of given service if it has been registered before.
        /// </summary>
        /// <typeparam name="TService">ServiceType</typeparam>
        /// <returns>Service instance of given type</returns>
        public static TService Get<TService>() where TService : class
        {
            Type serviceType = typeof(TService);

            FAssert.IsTrue(_registeredServices.ContainsKey(serviceType), $"{serviceType.Name} service does not exist.");

            return _registeredServices[serviceType] as TService;
        }

        public static void Remove<TService>() where TService : class
        {
            Remove(typeof(TService));
        }

        public static void Remove(Type serviceType)
        {
            _registeredServices.Remove(serviceType);
        }
        #endregion
    }
}