/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System.Threading.Tasks;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public interface IInitializable : INameable
    {
        #region Method
        /// <summary>
        /// PreInitialize is called when the script instance is being loaded.
        /// PreInitialize is used to initialize any variables or game state before the game starts.
        /// PreInitialize is called only once during the lifetime of the script instance.
        /// PreInitialize is called after all objects are initialized so you can safely speak to other objects or query them using eg. GameObject.FindWithTag.
        /// Each GameObject's PreInitialize is called in a random order between objects.
        /// Because of this, you should use PreInitialize to set up references between scripts, and use Initialize to pass any information back and forth.
        /// If you want to change execution order value, you should set it in here.
        /// PreInitialize is called once, just like the constructor.
        /// </summary>
        Task PreInitializeAsync();

        /// <summary>
        /// Initialize is called on the frame when a script is enabled just before any of the Tick methods is called the first time.
        /// Like the PreInitialize function, Initialize is called exactly once in the lifetime of the script.
        /// Each GameObject's Initialize is called based on the ExecutionOrder value.
        /// Initialize can be used as awaitable method. If you use it as sync method you must return Task.CompletedTask, otherwise await for desire method.
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// BeginPlay is called after all objects had been initialized and game is about to be ready.
        /// BeginPlay can be used as awaitable method. If you use it as sync method you must return Task.CompletedTask, otherwise await for desire method.
        /// In BeginPlay all objects had been initialized and you can send data back and forth to them and work with them.
        /// Tick callbacks can be set to invoke before BeginPlay.
        /// </summary>
        Task BeginPlayAsync();

        Task UninitializeAsync();
        #endregion
    }
}