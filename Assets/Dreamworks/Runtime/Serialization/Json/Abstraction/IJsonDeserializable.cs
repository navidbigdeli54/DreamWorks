namespace DreamMachineGameStudio.Dreamworks.Serialization.Json.Abstraction
{
    public interface IJsonDeserializable
    {
        void FromJson(FJsonObject jsonObject);
    }
}