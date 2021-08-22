/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine.Assertions.Comparers;

using UObject = UnityEngine.Object;

namespace DreamMachineGameStudio.Dreamworks.Debug
{
    public static class FAssert
    {
        #region Fields
        public readonly static Type CLASS_TYPE = typeof(FAssert);
        #endregion

        #region Methods
        /// <summary>
        /// Asserts that the condition is true.
        /// </summary>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="condition">true or false.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void IsTrue(bool condition)
        {
            if (!condition)
            {
                IsTrue(condition, null);
            }
        }

        /// <summary>
        /// Asserts that the condition is true.
        /// </summary>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="condition">true or false.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                LogError(BooleanFailureMessage(expected: true), message);
            }
        }

        /// <summary>
        /// Return true when the condition is false.  Otherwise return false.
        /// </summary>
        /// <param name="condition">true or false.</param>
        /// <param name="message">The string used to describe the result of the Assert.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void IsFalse(bool condition)
        {
            if (condition)
            {
                IsFalse(condition, null);
            }
        }

        /// <summary>
        /// Return true when the condition is false.  Otherwise return false
        /// </summary>
        /// <param name="condition">true or false.</param>
        /// <param name="message">The string used to describe the result of the Assert.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void IsFalse(bool condition, string message)
        {
            if (condition)
            {
                LogError(BooleanFailureMessage(expected: false), message);
            }
        }

        /// <summary>
        /// Assert the values are approximately equal
        /// </summary>
        /// <param name="tolerance">Tolerance of approximation.</param>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreApproximatelyEqual(float expected, float actual)
        {
            AreEqual(expected, actual, null, FloatComparer.s_ComparerWithDefaultTolerance);
        }

        /// <summary>
        /// Assert the values are approximately equal.
        /// </summary>
        /// <param name="tolerance">Tolerance of approximation.</param>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreApproximatelyEqual(float expected, float actual, string message)
        {
            AreEqual(expected, actual, message, FloatComparer.s_ComparerWithDefaultTolerance);
        }

        /// <summary>
        /// Assert the values are approximately equal.
        /// </summary>
        /// <param name="tolerance">Tolerance of approximation.</param>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreApproximatelyEqual(float expected, float actual, float tolerance)
        {
            AreApproximatelyEqual(expected, actual, tolerance, null);
        }

        /// <summary>
        /// Assert the values are approximately equal.
        /// </summary>
        /// <param name="tolerance">Tolerance of approximation.</param>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreApproximatelyEqual(float expected, float actual, float tolerance, string message)
        {
            AreEqual(expected, actual, message, new FloatComparer(tolerance));
        }

        /// <summary>
        /// Asserts that the values are approximately not equal.
        /// </summary>
        /// <param name="tolerance">Tolerance of approximation.</param>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotApproximatelyEqual(float expected, float actual)
        {
            AreNotEqual(expected, actual, null, FloatComparer.s_ComparerWithDefaultTolerance);
        }

        /// <summary>
        /// Asserts that the values are approximately not equal.
        /// </summary>
        /// <param name="tolerance">Tolerance of approximation.</param>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotApproximatelyEqual(float expected, float actual, string message)
        {
            AreNotEqual(expected, actual, message, FloatComparer.s_ComparerWithDefaultTolerance);
        }

        /// <summary>
        /// Asserts that the values are approximately not equal.
        /// </summary>
        /// <param name="tolerance">Tolerance of approximation.</param>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotApproximatelyEqual(float expected, float actual, float tolerance)
        {
            AreNotApproximatelyEqual(expected, actual, tolerance, null);
        }

        /// <summary>
        /// Asserts that the values are approximately not equal.
        /// </summary>
        /// <param name="tolerance">Tolerance of approximation.</param>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotApproximatelyEqual(float expected, float actual, float tolerance, string message)
        {
            AreNotEqual(expected, actual, message, new FloatComparer(tolerance));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual<T>(T expected, T actual)
        {
            AreEqual(expected, actual, null);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual<T>(T expected, T actual, string message)
        {
            AreEqual(expected, actual, message, EqualityComparer<T>.Default);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual<T>(T expected, T actual, string message, IEqualityComparer<T> comparer)
        {
            if (typeof(UObject).IsAssignableFrom(typeof(T)))
            {
                AreEqual(expected as UObject, actual as UObject, message);
            }
            else if (!comparer.Equals(actual, expected))
            {
                LogError(GetEqualityMessage(actual, expected, expectEqual: true), message);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(UObject expected, UObject actual, string message)
        {
            if (actual != expected)
            {
                LogError(GetEqualityMessage(actual, expected, expectEqual: true), message);
            }
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual<T>(T expected, T actual)
        {
            AreNotEqual(expected, actual, null);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual<T>(T expected, T actual, string message)
        {
            AreNotEqual(expected, actual, message, EqualityComparer<T>.Default);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual<T>(T expected, T actual, string message, IEqualityComparer<T> comparer)
        {
            if (typeof(UObject).IsAssignableFrom(typeof(T)))
            {
                AreNotEqual(expected as UObject, actual as UObject, message);
            }
            else if (comparer.Equals(actual, expected))
            {
                LogError(GetEqualityMessage(actual, expected, expectEqual: false), message);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(UObject expected, UObject actual, string message)
        {
            if (actual == expected)
            {
                LogError(GetEqualityMessage(actual, expected, expectEqual: false), message);
            }
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void IsNull<T>(T value) where T : class
        {
            IsNull(value, null);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void IsNull<T>(T value, string message) where T : class
        {
            if (typeof(UObject).IsAssignableFrom(typeof(T)))
            {
                IsNull(value as UObject, message);
            }
            else if (value != null)
            {
                LogError(NullFailureMessage(true), message);
            }
        }

        /// <summary>
        /// Assert the value is null.
        /// </summary>
        /// <param name="value">The Object or type being checked for.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void IsNull(UObject value, string message)
        {
            if (value != null)
            {
                LogError(NullFailureMessage(true), message);
            }
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void IsNotNull<T>(T value) where T : class
        {
            IsNotNull(value, null);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void IsNotNull<T>(T value, string message) where T : class
        {
            if (typeof(UObject).IsAssignableFrom(typeof(T)))
            {
                IsNotNull(value as UObject, message);
            }
            else if (value == null)
            {
                LogError(NullFailureMessage(false), message);
            }
        }

        /// <summary>
        /// Assert that the value is not null.
        /// </summary>
        /// <param name="value">The Object or type being checked for.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void IsNotNull(UObject value, string message)
        {
            if (value == null)
            {
                LogError(NullFailureMessage(false), message);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(sbyte expected, sbyte actual)
        {
            if (expected != actual)
            {
                AreEqual<sbyte>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(sbyte expected, sbyte actual, string message)
        {
            if (expected != actual)
            {
                AreEqual<sbyte>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(sbyte expected, sbyte actual)
        {
            if (expected == actual)
            {
                AreNotEqual<sbyte>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(sbyte expected, sbyte actual, string message)
        {
            if (expected == actual)
            {
                AreNotEqual<sbyte>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(byte expected, byte actual)
        {
            if (expected != actual)
            {
                AreEqual<byte>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(byte expected, byte actual, string message)
        {
            if (expected != actual)
            {
                AreEqual<byte>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(byte expected, byte actual)
        {
            if (expected == actual)
            {
                AreNotEqual<byte>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(byte expected, byte actual, string message)
        {
            if (expected == actual)
            {
                AreNotEqual<byte>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(char expected, char actual)
        {
            if (expected != actual)
            {
                AreEqual<char>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(char expected, char actual, string message)
        {
            if (expected != actual)
            {
                AreEqual<char>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(char expected, char actual)
        {
            if (expected == actual)
            {
                AreNotEqual<char>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(char expected, char actual, string message)
        {
            if (expected == actual)
            {
                AreNotEqual<char>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(short expected, short actual)
        {
            if (expected != actual)
            {
                AreEqual<short>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(short expected, short actual, string message)
        {
            if (expected != actual)
            {
                AreEqual<short>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(short expected, short actual)
        {
            if (expected == actual)
            {
                AreNotEqual<short>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(short expected, short actual, string message)
        {
            if (expected == actual)
            {
                AreNotEqual<short>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(ushort expected, ushort actual)
        {
            if (expected != actual)
            {
                AreEqual<ushort>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(ushort expected, ushort actual, string message)
        {
            if (expected != actual)
            {
                AreEqual<ushort>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(ushort expected, ushort actual)
        {
            if (expected == actual)
            {
                AreNotEqual<ushort>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(ushort expected, ushort actual, string message)
        {
            if (expected == actual)
            {
                AreNotEqual<ushort>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(int expected, int actual)
        {
            if (expected != actual)
            {
                AreEqual<int>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(int expected, int actual, string message)
        {
            if (expected != actual)
            {
                AreEqual<int>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(int expected, int actual)
        {
            if (expected == actual)
            {
                AreNotEqual<int>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(int expected, int actual, string message)
        {
            if (expected == actual)
            {
                AreNotEqual<int>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(uint expected, uint actual)
        {
            if (expected != actual)
            {
                AreEqual<uint>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(uint expected, uint actual, string message)
        {
            if (expected != actual)
            {
                AreEqual<uint>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(uint expected, uint actual)
        {
            if (expected == actual)
            {
                AreNotEqual<uint>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(uint expected, uint actual, string message)
        {
            if (expected == actual)
            {
                AreNotEqual<uint>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(long expected, long actual)
        {
            if (expected != actual)
            {
                AreEqual<long>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(long expected, long actual, string message)
        {
            if (expected != actual)
            {
                AreEqual<long>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(long expected, long actual)
        {
            if (expected == actual)
            {
                AreNotEqual<long>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(long expected, long actual, string message)
        {
            if (expected == actual)
            {
                AreNotEqual<long>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(ulong expected, ulong actual)
        {
            if (expected != actual)
            {
                AreEqual<ulong>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreEqual(ulong expected, ulong actual, string message)
        {
            if (expected != actual)
            {
                AreEqual<ulong>(expected, actual, message);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(ulong expected, ulong actual)
        {
            if (expected == actual)
            {
                AreNotEqual<ulong>(expected, actual, (string)null);
            }
        }

        /// <summary>
        /// Assert that the values are not equal.
        /// </summary>
        /// <param name="expected">The assumed Assert value.</param>
        /// <param name="actual">The exact Assert value.</param>
        /// <param name="message">The string used to describe the Assert.</param>
        /// <param name="comparer">Method to compare expected and actual arguments have the same value.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AreNotEqual(ulong expected, ulong actual, string message)
        {
            if (expected == actual)
            {
                AreNotEqual<ulong>(expected, actual, message);
            }
        }
        #endregion

        #region Private Methods
        private static void LogError(string message, string userMessage)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Assertion has failed\n";
            }

            if (string.IsNullOrEmpty(userMessage) == false)
            {
                message = $"{message} | {userMessage}";
            }

            FLog.Error(CLASS_TYPE.Name, message);
        }

        private static string GetMessage(string failureMessage, string expected) => $"{failureMessage}{Environment.NewLine}, Expected: {expected}";

        private static string NullFailureMessage(bool expectNull) => GetMessage($"Value was {(expectNull ? "not" : string.Empty)}Null.", $"Value was {(expectNull ? "" : "not")}Null");

        private static string BooleanFailureMessage(bool expected) => GetMessage($"Value was {!expected}", $"{expected}");

        private static string GetEqualityMessage(object actual, object expected, bool expectEqual) => GetMessage($"Values are {(expectEqual ? "not" : string.Empty)}equal.", $"{actual} {expected} {(expectEqual ? "==" : "!=")}");
        #endregion

        #region Obsolete Methods
        [Obsolete("FAssert.Equals should not be used for Assertions", true)]
        public new static bool Equals(object obj1, object obj2)
        {
            throw new InvalidOperationException("Assert.Equals should not be used for Assertions");
        }

        [Obsolete("FAssert.ReferenceEquals should not be used for Assertions", true)]
        public new static bool ReferenceEquals(object obj1, object obj2)
        {
            throw new InvalidOperationException("Assert.ReferenceEquals should not be used for Assertions");
        }
        #endregion
    }
}