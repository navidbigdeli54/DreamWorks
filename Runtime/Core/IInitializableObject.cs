/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public interface IInitializableObject : IInitializable
    {
        #region Property
        /// <summary>
        /// If true, this object has been registered to the framework, otherwise not.
        /// </summary>
        bool HasRegistered { get; }

        /// <summary>
        /// If true, object's PreInitializeComponent and InitializeComponent has been called, otherwise not.
        /// </summary>
        bool HasInitialized { get; }

        /// <summary>
        /// If true, object's BeginPlay has been called, otherwise not.
        /// </summary>
        bool HasBegunPlay { get; }

        bool HasEndedPlay { get; }

        bool IsTransient { get; }
        #endregion
    }
}