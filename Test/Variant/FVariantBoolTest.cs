/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using NUnit.Framework;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.Variant;

namespace DreamMachineGameStudio.Dreamworks.Test.Variant
{
    public class FVariantBoolTest
    {
        [Test]
        public void CompareTwoFalseBoolTest()
        {
            FBool left = false;

            FBool right = false;

            FAssert.AreEqual(left, right);
        }

        [Test]
        public void CompreAFalseAndATrueBoolTest()
        {
            FBool left = false;

            FBool right = true;

            FAssert.AreNotEqual(left, right);
        }
    }
}