/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.Serialization.Json
{
    public interface IJsonDeserializable
    {
        void FromJson(FJsonObject jsonObject);
    }
}