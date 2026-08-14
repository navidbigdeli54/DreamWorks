namespace DreamMachineGameStudio.Dreamworks.Serialization.Json.Abstraction
{
    public interface IJsonSerializable
    {
        int Version { get; }

        FJsonObject ToJson();
    }
}