using System;
using UnityEngine;
using DreamMachineGameStudio.DreamWorks.LogProvider;

namespace DreamMachineGameStudio.DreamWorks.Foundation
{
    /// <summary>
    /// Represents a serializable wrapper for a type that is constrained to a specific base class or interface.
    /// </summary>
    /// <remarks>This class allows storing and manipulating a type reference constrained to a specific base
    /// class or interface. It provides functionality to serialize the type information and construct instances of the
    /// specified type.</remarks>
    /// <typeparam name="T">The base class or interface that the type must derive from or implement. Must be a reference type.</typeparam>
    [Serializable]
    public class TSubclassOf<T> where T : class
    {
        #region Fields
        [SerializeField]
        private string assemblyQualifiedTypeName;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="Type"/> represented by the assembly-qualified type name.
        /// </summary>
        /// <remarks>When setting this property, the assembly-qualified name of the specified <see
        /// cref="Type"/> is stored internally. When getting this property, the <see cref="Type"/> is resolved from the
        /// stored assembly-qualified name.</remarks>
        public Type Type
        {
            get
            {
                if (string.IsNullOrEmpty(assemblyQualifiedTypeName))
                {
                    return null;
                }

                return Type.GetType(assemblyQualifiedTypeName);
            }
            set
            {
                assemblyQualifiedTypeName = value?.AssemblyQualifiedName;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Implicitly converts a <see cref="TSubclassOf{T}"/> instance to its associated <see cref="Type"/>.
        /// </summary>
        /// <param name="subclass">The <see cref="TSubclassOf{T}"/> instance to convert. Can be <see langword="null"/>.</param>
        public static implicit operator Type(TSubclassOf<T> subclass)
        {
            return subclass?.Type;
        }

        /// <summary>
        /// Defines an implicit conversion from a <see cref="Type"/> to a <see cref="TSubclassOf{T}"/>.
        /// </summary>
        /// <remarks>This operator allows a <see cref="Type"/> to be implicitly converted to a <see
        /// cref="TSubclassOf{T}"/>. The resulting instance contains the assembly-qualified name of the specified
        /// type.</remarks>
        /// <param name="type">The <see cref="Type"/> to be converted. Must represent a type that is assignable to <typeparamref
        /// name="T"/>.</param>
        public static implicit operator TSubclassOf<T>(Type type)
        {
            return new TSubclassOf<T>() { assemblyQualifiedTypeName = type.AssemblyQualifiedName };
        }

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <returns>The name of the type represented by the current object.</returns>
        public override string ToString()
        {
            return Type.Name;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates an instance of the specified type using the provided constructor arguments.
        /// </summary>
        /// <remarks>This method uses <see cref="Activator.CreateInstance"/> to create the instance. If an
        /// error occurs during instantiation, the error is logged, and <see langword="null"/> is returned.</remarks>
        /// <param name="args">An array of arguments to pass to the constructor of the specified type. Can be null or empty if the
        /// constructor does not require parameters.</param>
        /// <returns>An instance of the specified type, or <see langword="null"/> if the instance could not be created.</returns>
        public T Construct(object[] args)
        {
            try
            {
                return Activator.CreateInstance(Type, args) as T;
            }
            catch (Exception exception)
            {
                FDefaultLogger.Instance.LogError(exception, $"Encounter an error creating instance of {Type}");

                return null;
            }
        }
        #endregion
    }
}