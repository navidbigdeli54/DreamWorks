namespace DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction
{
    public interface IJsonDeserializable
    {
        void FromJson(FJsonObject jsonObject);
    }
}