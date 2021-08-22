/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using NUnit.Framework;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.Variant;

namespace DreamMachineGameStudio.Dreamworks.Test.Variant
{
    public class FVariantIntTest
    {
        [Test]
        public void CompareTwoSameIntTest()
        {
            FInt left = 1;

            FInt right = 1;

            FAssert.AreEqual(left, right);
        }

        [Test]
        public void CompareTwoDifferentIntTest()
        {
            FInt left = 1;

            FInt right = 2;

            FAssert.AreNotEqual(left, right);
        }
    }
}