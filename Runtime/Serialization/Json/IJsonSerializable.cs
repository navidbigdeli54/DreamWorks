/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.Serialization.Json
{
    public interface IJsonSerializable
    {
        int Version { get; }

        FJsonObject ToJson();
    }
}