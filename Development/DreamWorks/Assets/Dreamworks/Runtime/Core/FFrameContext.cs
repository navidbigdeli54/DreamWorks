namespace DreamMachineGameStudio.DreamWorks.Core
{
    public struct FFrameContext
    {
        public float DeltaTime { get; private set; }

        public long FrameNumber { get; private set; }

        public FFrameContext(float deltaTime, long frameNumber)
        {
            DeltaTime = deltaTime;
            FrameNumber = frameNumber;
        }
    }
}