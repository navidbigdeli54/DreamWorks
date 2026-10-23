namespace DreamMachineGameStudio.DreamWorks.Application
{
    /// <summary>
    /// Represents the context of a single frame in a simulation or rendering loop.
    /// </summary>
    /// <remarks>This structure provides information about the current frame, including the time elapsed since
    /// the previous frame and the sequential frame number. It is commonly used in scenarios such as game loops,
    /// animations, or simulations to track frame-specific data.</remarks>
    public struct FFrameContext
    {
        #region Properties
        /// <summary>
        /// Gets the time, in seconds, that has elapsed since the last frame.
        /// </summary>
        public float DeltaTime { get; private set; }

        /// <summary>
        /// Gets the current frame number in the sequence.
        /// </summary>
        public long FrameNumber { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Represents the context of a single frame in a simulation or rendering loop.
        /// </summary>
        /// <remarks>This constructor initializes the frame context with the specified time delta and
        /// frame number. The <paramref name="deltaTime"/> parameter is typically used to calculate time-dependent
        /// updates, while <paramref name="frameNumber"/> can be used to track the progression of frames in a
        /// loop.</remarks>
        /// <param name="deltaTime">The time, in seconds, that has elapsed since the previous frame. Must be a non-negative value.</param>
        /// <param name="frameNumber">The sequential number of the current frame. Must be a non-negative value.</param>
        public FFrameContext(float deltaTime, long frameNumber)
        {
            DeltaTime = deltaTime;
            FrameNumber = frameNumber;
        } 
        #endregion
    }
}