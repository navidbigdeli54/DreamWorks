/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Utility
{
    public static class FReflectionUtility
    {
        #region Field
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

        private static readonly IDictionary<Assembly, Type[]> _assemblies = new Dictionary<Assembly, Type[]>();
        #endregion

        #region Property
        public static Type CLASS_TYPE { get; } = typeof(FReflectionUtility);
        #endregion

        #region Constructor
        static FReflectionUtility()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                _assemblies.Add(assembly, assembly.GetTypes());
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// Is given class type defines following attributes on it self?
        /// </summary>
        /// <param name="classType">Type of the class that you want to check.</param>
        /// <param name="attributeType">Type of attribute that you want to check.</param>
        /// <returns></returns>
        public static bool HasDefinedAttribute(Type classType, Type attributeType)
        {
            FAssert.IsNotNull(classType);
            FAssert.IsNotNull(attributeType);
            FAssert.IsTrue(attributeType.IsSubclassOf(typeof(Attribute)));

            return classType.IsDefined(attributeType);
        }

        /// <summary>
        /// Get all types in given assembly that inherited from given type.
        /// </summary>
        /// <typeparam name="T">Desire type</typeparam>
        /// <param name="assemblyName">Desire assembly</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetSubTypesOf<T>()
        {
            return GetSubTypesOf(typeof(T));
        }

        public static IEnumerable<Type> GetSubTypesOf(Type type)
        {
            List<Type> result = new List<Type>();

            foreach (var assemblyTypes in _assemblies.Values)
            {
                if (type.IsInterface)
                {
                    result.AddRange(assemblyTypes.Where(row => row.GetInterface(type.Name) != null));
                }
                else
                {
                    result.AddRange(assemblyTypes.Where(row => row.IsSubclassOf(type)));
                }
            }

            return result;
        }

        public static Type GetType(string typeName)
        {
            foreach (Type[] assemblyTypes in _assemblies.Values)
            {
                Type desireType = assemblyTypes.Where(x => x.FullName.Equals(typeName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

                if (desireType != null) return desireType;
            }

            return null;
        }

        /// <summary>
        /// If given instance's class has defines given attribute, return desire property.
        /// </summary>
        /// <returns>Value of desire property in given attribute</returns>
        public static T GetAttributeProperty<T>(object instance, Type attributeType, string propertyName)
        {
            FAssert.IsNotNull(instance);
            FAssert.IsNotNull(attributeType);
            FAssert.IsFalse(string.IsNullOrEmpty(propertyName));
            FAssert.IsTrue(attributeType.IsSubclassOf(typeof(Attribute)));

            Type instanceType = instance.GetType();

            FAssert.IsTrue(HasDefinedAttribute(instanceType, attributeType));

            PropertyInfo propertyInfo = attributeType.GetProperty(propertyName, BINDING_FLAGS);

            FAssert.IsNotNull(propertyInfo);

            try
            {
                Attribute attribute = instanceType.GetCustomAttribute(attributeType);

                return (T)propertyInfo.GetValue(attribute);
            }
            catch (InvalidCastException exception)
            {
                FLog.Error(CLASS_TYPE.Name, exception.Message);

                return default;
            }
        }

        public static T GetAttributeProperty<T>(Type classType, Type attributeType, string propertyName)
        {
            FAssert.IsNotNull(classType);
            FAssert.IsNotNull(attributeType);
            FAssert.IsFalse(string.IsNullOrEmpty(propertyName));
            FAssert.IsTrue(attributeType.IsSubclassOf(typeof(Attribute)));

            FAssert.IsTrue(HasDefinedAttribute(classType, attributeType));

            PropertyInfo propertyInfo = attributeType.GetProperty(propertyName, BINDING_FLAGS);

            FAssert.IsNotNull(propertyInfo);

            try
            {
                return (T)propertyInfo.GetValue(classType.GetCustomAttribute(attributeType));
            }
            catch (InvalidCastException exception)
            {
                FLog.Error(CLASS_TYPE.Name, exception.Message);

                return default;
            }
        }

        /// <summary>
        /// Set given value to the given property name in desire instance.
        /// </summary>
        /// <param name="instance">Desire instance</param>
        /// <param name="propertyName">The name of desire property</param>
        /// <param name="value">Desire value for given property.</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            FAssert.IsNotNull(instance);
            FAssert.IsFalse(string.IsNullOrEmpty(propertyName));

            Type instanceType = instance.GetType();

            PropertyInfo propertyInfo = instanceType.GetProperty(propertyName, BINDING_FLAGS);

            FAssert.IsNotNull(propertyInfo);
            FAssert.IsTrue(propertyInfo.PropertyType.IsAssignableFrom(value.GetType()));

            if (propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(instance, value);
                return;
            }

            Type DeclaringType = propertyInfo.DeclaringType;

            PropertyInfo declaringPropertyInfo = DeclaringType.GetProperty(propertyName, BINDING_FLAGS);

            FAssert.IsNotNull(declaringPropertyInfo);

            try
            {
                declaringPropertyInfo.SetValue(instance, value);
            }
            catch (Exception exception)
            {
                FLog.Error(CLASS_TYPE.Name, exception.Message);
            }
        }

        /// <summary>
        /// Set given value to the given property in desire instance.
        /// </summary>
        /// <param name="instance">Desire instance</param>
        /// <param name="propertyInfo">The property info of desire property</param>
        /// <param name="value">Desire value for given property.</param>
        public static void SetProperty(object instance, PropertyInfo propertyInfo, object value)
        {
            FAssert.IsNotNull(instance);
            FAssert.IsNotNull(propertyInfo);

            FAssert.AreEqual(propertyInfo.DeclaringType, instance.GetType());
            FAssert.IsTrue(propertyInfo.PropertyType.IsAssignableFrom(value.GetType()));

            if (propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(instance, value);
                return;
            }

            Type DeclaringType = propertyInfo.DeclaringType;

            PropertyInfo declaringPropertyInfo = DeclaringType.GetProperty(propertyInfo.Name, BINDING_FLAGS);

            FAssert.IsNotNull(declaringPropertyInfo);

            try
            {
                declaringPropertyInfo.SetValue(instance, value);
            }
            catch (Exception exception)
            {
                FLog.Error(CLASS_TYPE.Name, exception.Message);
            }
        }

        /// <summary>
        /// Set given value to the given field name in desire instance.
        /// </summary>
        /// <param name="instance">Desire instance</param>
        /// <param name="fieldName">The name of desire property</param>
        /// <param name="value">Desire value for given property.</param>
        public static void SetField(object instance, string fieldName, object value)
        {
            FAssert.IsNotNull(instance);
            FAssert.IsFalse(string.IsNullOrEmpty(fieldName));

            Type instanceType = instance.GetType();

            FieldInfo fieldInfo = instanceType.GetField(fieldName, BINDING_FLAGS);

            FAssert.IsNotNull(fieldInfo);

            try
            {
                fieldInfo.SetValue(instance, value);
            }
            catch (Exception exception)
            {
                FLog.Error(CLASS_TYPE.Name, exception.Message);
            }
        }

        /// <summary>
        /// Set given value to the given field in desire instance.
        /// </summary>
        /// <param name="instance">Desire instance</param>
        /// <param name="fieldInfo">The field info of desire property</param>
        /// <param name="value">Desire value for given property.</param>
        public static void SetField(object instance, FieldInfo fieldInfo, object value)
        {
            FAssert.IsNotNull(instance);
            FAssert.IsNotNull(fieldInfo);

            try
            {
                fieldInfo.SetValue(instance, value);
            }
            catch (Exception exception)
            {
                FLog.Error(CLASS_TYPE.Name, exception.Message);
            }
        }

        /// <summary>
        /// Returns all properties of given object.(Any access level).
        /// </summary>
        public static IEnumerable<PropertyInfo> GetAllProperties(object obj)
        {
            Type type = obj.GetType();

            return type.GetProperties(BINDING_FLAGS);
        }

        /// <summary>
        /// Returns all fields of given object.(Any access level).
        /// </summary>
        public static IEnumerable<FieldInfo> GetAllFields(object obj)
        {
            Type type = obj.GetType();

            return type.GetFields(BINDING_FLAGS);
        }

        public static FieldInfo GetField(object obj, string fieldName)
        {
            Type type = obj.GetType();

            return type.GetField(fieldName, BINDING_FLAGS);
        }

        /// <summary>
        /// Returns all interfaces of given object.
        /// </summary>
        public static IEnumerable<Type> GetAllInterfaces(object obj)
        {
            Type type = obj.GetType();

            return type.GetInterfaces();
        }

        /// <summary>
        /// Returns all interfaces of given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllInterfaces(Type type)
        {
            return type.GetInterfaces();
        }
        #endregion
    }
}