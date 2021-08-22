/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using NUnit.Framework;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.Variant;

namespace DreamMachineGameStudio.Dreamworks.Test.Variant
{
    public class FVariantStringTest
    {
        [Test]
        public void CompareTwoSameStringTest()
        {
            FString left = "A";

            FString right = "A";

            FAssert.AreEqual(left, right);
        }

        [Test]
        public void CompareTwoDifferentStringTest()
        {
            FString left = "A";

            FString right = "B";

            FAssert.AreNotEqual(left, right);
        }
    }
}