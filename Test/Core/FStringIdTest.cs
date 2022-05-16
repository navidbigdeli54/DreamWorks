/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

using System;
using NUnit.Framework;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Test.Core
{
    public class FStringIdTest
    {
        [Test]
        public void EqualityTest()
        {
            string random = Guid.NewGuid().ToString();

            FStringId stringId1 = new FStringId(random);
            FStringId stringId2 = new FStringId(random);

            FAssert.AreEqual(stringId1, stringId2);
        }

        [Test]
        public void ToStringTest()
        {
            string random = Guid.NewGuid().ToString();

            FStringId stringId1 = new FStringId(random);
            FStringId stringId2 = new FStringId(random);

            FAssert.AreEqual(stringId1.ToString(), stringId2.ToString());
            FAssert.AreEqual(random, stringId1.ToString());
            FAssert.AreEqual(random, stringId2.ToString());
        }

        [Test]
        public void GetHashCodeTest()
        {
            string random = Guid.NewGuid().ToString();

            FStringId stringId1 = new FStringId(random);
            FStringId stringId2 = new FStringId(random);

            FAssert.AreEqual(stringId1.GetHashCode(), stringId2.GetHashCode());
        }

        [Test]
        public void EqualTest()
        {
            string random = Guid.NewGuid().ToString();

            FStringId stringId1 = new FStringId(random);
            FStringId stringId2 = new FStringId(random);

            FAssert.IsTrue(stringId1.Equals(stringId2));
        }
    }
}
