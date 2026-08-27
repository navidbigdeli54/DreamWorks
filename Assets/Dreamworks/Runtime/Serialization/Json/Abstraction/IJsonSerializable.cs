namespace DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction
{
    public interface IJsonSerializable
    {
        int Version { get; }

        FJsonObject ToJson();
    }
}