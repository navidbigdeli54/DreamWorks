/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public interface ITickable : INameable
    {
        #region Methods
        /// <summary>
        /// Tick is called every frame.
        /// Tick is the most commonly used function to implement any kind of game behavior.
        /// </summary>
        /// <param name="deltaTime">The time in seconds it took to complete the last frame.</param>
        void Tick(float deltaTime);

        /// <summary>
        /// LateTick is called every frame.
        /// LateTick is called after all Tick functions have been called.This is useful to order script execution.
        /// For example a follow camera should always be implemented in LateTick because it tracks objects that might have moved inside Tick.
        /// </summary>
        /// <param name="deltaTime">The time in seconds it took to complete the last frame.</param>
        void LateTick(float deltaTime);

        /// <summary>
        /// This function is called every fixed framerate frame.
        /// FixedTick should be used instead of Update when dealing with Rigidbody.
        /// For example when adding a force to a rigidbody, you have to apply the force every fixed frame inside FixedTick instead of every frame inside Tick.
        /// </summary>
        /// <param name="fixedDeltaTime">The interval in seconds at which physics and other fixed frame rate updates (like MonoBehaviour's FixedUpdate) are performed.</param>
        void FixedTick(float fixedDeltaTime);
        #endregion
    }
}