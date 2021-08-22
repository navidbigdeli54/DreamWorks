/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using NUnit.Framework;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.Variant;

namespace DreamMachineGameStudio.Dreamworks.Test.Variant
{
    public class FVariantFloatTest
    {
        [Test]
        public void CompareTwoSameFloatTest()
        {
            FFloat left = 1;

            FFloat right = 1;

            FAssert.AreEqual(left, right);
        }

        [Test]
        public void CompareTwoDifferentFloatTest()
        {
            FFloat left = 1;

            FFloat right = 2;

            FAssert.AreNotEqual(left, right);
        }
    }
}