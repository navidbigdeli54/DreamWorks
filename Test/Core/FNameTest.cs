/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

using System;
using NUnit.Framework;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Test.Core
{
    public class FNameTest
    {
        [Test]
        public void EqualityCheckTest()
        {
            string random = Guid.NewGuid().ToString();

            FName name1 = new FName(random);
            FName name2 = new FName(random);

            FAssert.AreEqual(name1, name2);
            
        }

        [Test]
        public void GetHashCodeTest()
        {
            string random = Guid.NewGuid().ToString();

            FName name1 = new FName(random);
            FName name2 = new FName(random);

            FAssert.AreEqual(name1.GetHashCode(), name2.GetHashCode());
        }

        [Test]
        public void ToStringTest()
        {
            string random = Guid.NewGuid().ToString();

            FName name1 = new FName(random);
            FName name2 = new FName(random);

            FAssert.AreEqual(name1.ToString(), name2.ToString());
            FAssert.AreEqual(random, name1.ToString());
            FAssert.AreEqual(random, name2.ToString());
            FAssert.IsTrue(name1 == name2);
            FAssert.IsFalse(name2 != name1);
        }

        [Test]
        public void EqualOperatorTest()
        {
            string random = Guid.NewGuid().ToString();

            FName name1 = new FName(random);
            FName name2 = new FName(random);

            FAssert.IsTrue(name1 == name2);
        }

        [Test]
        public void NotEqualOperatorTest()
        {
            string random = Guid.NewGuid().ToString();

            FName name1 = new FName(random);
            FName name2 = new FName(random);

            FAssert.IsTrue(name1 == name2);
            FAssert.IsFalse(name1 != name2);
        }

        [Test]
        public void EqualTest()
        {
            string random = Guid.NewGuid().ToString();

            FName name1 = new FName(random);
            FName name2 = new FName(random);

            FAssert.IsTrue(name1.Equals(name2));
            FAssert.IsTrue(name2.Equals(name1));
        }
    }
}
