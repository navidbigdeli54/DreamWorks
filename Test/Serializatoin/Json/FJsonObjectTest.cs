/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

using System;
using NUnit.Framework;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.Serialization.Json;

namespace DreamMachineGameStudio.Dreamworks.Test
{
    public class FJsonObjectTest
    {
        [Test]
        public void ParseString()
        {
            string jsonString = "{ \"Name\" : \"DreamMachine\" }";
            FJsonNode jsonNode = FJsonNode.Parse(jsonString);
            FJsonObject jsonObject = jsonNode as FJsonObject;
            FAssert.IsNotNull(jsonObject);
            FAssert.AreEqual<string>("DreamMachine", jsonObject["Name"]);
        }

        [Test]
        public void SerializeString()
        {
            FJsonObject jsonObject = new FJsonObject();
            jsonObject["Name"] = "DreamMachine";
            string jsonString = jsonObject.ToString();
            FAssert.AreEqual($"{{{Environment.NewLine}    \"Name\" : \"DreamMachine\"{Environment.NewLine}}}", jsonString);
        }

        [Test]
        public void ParseNumber()
        {
            string jsonString = "{ \"Age\" : 10 }";
            FJsonNode jsonNode = FJsonNode.Parse(jsonString);
            FJsonObject jsonObject = jsonNode as FJsonObject;
            FAssert.IsNotNull(jsonObject);
            FAssert.AreEqual<double>(10, jsonObject["Age"]);
        }

        [Test]
        public void SerializeNumber()
        {
            FJsonObject jsonObject = new FJsonObject();
            jsonObject["Age"] = 10;
            string jsonString = jsonObject.ToString();
            FAssert.AreEqual($"{{{Environment.NewLine}    \"Age\" : 10{Environment.NewLine}}}", jsonString);
        }

        [Test]
        public void ParseBoolean()
        {
            string jsonString = "{\"IsActive\" : true }";
            FJsonNode jsonNode = FJsonNode.Parse(jsonString);
            FJsonObject jsonObject = jsonNode as FJsonObject;
            FAssert.IsNotNull(jsonObject);
            FAssert.AreEqual<bool>(true, jsonObject["IsActive"]);

        }

        [Test]
        public void SerializeBoolean()
        {
            FJsonObject jsonObject = new FJsonObject();
            jsonObject["IsActive"] = true;
            string jsonString = jsonObject.ToString();
            FAssert.AreEqual($"{{{Environment.NewLine}    \"IsActive\" : true{Environment.NewLine}}}", jsonString);
        }

        [Test]
        public void ParseNull()
        {
            string jsonString = "{\"Owner\" : null }";
            FJsonNode jsonNode = FJsonNode.Parse(jsonString);
            FJsonObject jsonObject = jsonNode as FJsonObject;
            FAssert.IsNotNull(jsonObject);
            FAssert.AreEqual(FJsonNull.NULL, jsonObject["Owner"]);
        }

        [Test]
        public void SerializeNull()
        {
            FJsonObject jsonObject = new FJsonObject();
            jsonObject["Owner"] = null;
            string jsonString = jsonObject.ToString();
            FAssert.AreEqual($"{{{Environment.NewLine}    \"Owner\" : null{Environment.NewLine}}}", jsonString);
        }

        [Test]
        public void ParseArray()
        {
            string jsonString = "{ \"Products\" : [\"DreamWorks\", \"MiniDizi\"] }";
            FJsonNode jsonNode = FJsonNode.Parse(jsonString);
            FJsonObject jsonObject = jsonNode as FJsonObject;
            FAssert.IsNotNull(jsonObject);
            FJsonArray jsonArray = jsonObject["Products"] as FJsonArray;
            FAssert.IsNotNull(jsonArray);
            FAssert.AreEqual(2, jsonArray.Count);
            FAssert.AreEqual<string>("DreamWorks", jsonArray[0]);
            FAssert.AreEqual<string>("MiniDizi", jsonArray[1]);
        }

        [Test]
        public void SerializeArray()
        {
            FJsonArray jsonArray = new FJsonArray();
            jsonArray.Add("DreamWorks");
            jsonArray.Add("MiniDizi");
            FJsonObject jsonObject = new FJsonObject();
            jsonObject["Products"] = jsonArray;
            string jsonString = jsonObject.ToString();
            FAssert.AreEqual($"{{{Environment.NewLine}    \"Products\" : [{Environment.NewLine}        \"DreamWorks\",{Environment.NewLine}        \"MiniDizi\"{Environment.NewLine}    ]{Environment.NewLine}}}", jsonString);
        }
    }
}