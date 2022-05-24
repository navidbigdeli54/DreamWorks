/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

using System;
using NUnit.Framework;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.Variant;
using DreamMachineGameStudio.Dreamworks.Blackboard;

namespace DreamMachineGameStudio.Dreamworks.Test.Blackboard
{
    public class FBlackboardTest
    {
        [Test]
        public void GetValueTest()
        {
            string key = Guid.NewGuid().ToString();
            string value = Guid.NewGuid().ToString();

            FBlackboard blackboard = new FBlackboard();
            blackboard.AddValue(key, new FString(value));

            FString variant = blackboard.GetValue<FString>(key);

            FAssert.AreEqual(new FString(value), variant);
        }

        [Test]
        public void TryGetValueTest()
        {
            string key = Guid.NewGuid().ToString();
            string value = Guid.NewGuid().ToString();

            FBlackboard blackboard = new FBlackboard();
            blackboard.AddValue(key, new FString(value));

            FAssert.IsTrue(blackboard.TryGetValue(key, out FString variant));
            FAssert.AreEqual(new FString(value), variant);
        }
    }
}