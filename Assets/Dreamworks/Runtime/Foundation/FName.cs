using System;
using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Foundation
{
    /// <summary>
    /// Represents a globally unique, case-sensitive name that can be efficiently compared and stored.
    /// </summary>
    /// <remarks>The <see cref="FName"/> struct is designed to provide a lightweight, immutable representation
    /// of a name that is globally unique and optimized for comparisons. Names are registered in an internal pool,
    /// ensuring that repeated instances of the same name share the same underlying identifier. This makes <see
    /// cref="FName"/> suitable for scenarios where frequent name comparisons or lookups are required, such as in game
    /// engines or other performance-critical applications.  Instances of <see cref="FName"/> can be implicitly created
    /// from strings, and the <see cref="ToString"/> method retrieves the original string representation of the name.
    /// The struct also supports equality and comparison operations.</remarks>
    public readonly struct FName : IEquatable<FName>, IComparable<FName>
    {
        #region Fields
        private readonly int id;

        private static readonly FNamePool pool = new FNamePool();
        #endregion

        #region Properties
        public static FName None => default;

        public bool IsNone => id == 0;

        internal int Id => id;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FName"/> class with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier for the instance.</param>
        private FName(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FName"/> class with the specified string value.
        /// </summary>
        /// <remarks>The provided <paramref name="value"/> is registered in an internal pool to ensure
        /// efficient reuse of string values.</remarks>
        /// <param name="value">The string value to associate with this instance. Cannot be <see langword="null"/> or empty.</param>
        public FName(string value)
        {
            id = pool.Register(value);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <remarks>The returned string is retrieved from a string pool using the object's identifier.
        /// Ensure that the identifier is valid and corresponds to an entry in the pool.</remarks>
        /// <returns>A string representation of the object, or an empty string if the identifier does not correspond to a valid
        /// entry in the pool.</returns>
        public override string ToString()
        {
            return pool.GetString(id);
        }

        /// <summary>
        /// Returns the hash code for the current object.
        /// </summary>
        /// <remarks>The hash code is derived from the object's <c>id</c> field.  This ensures that
        /// objects with the same <c>id</c> will have the same hash code.</remarks>
        /// <returns>An integer representing the hash code of the current object.</returns>
        public override int GetHashCode()
        {
            return id;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified object is of type <c>FName</c> and is equal to the current instance;
        /// otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is FName other && Equals(other);
        }

        /// <summary>
        /// Determines whether the current instance is equal to another instance of the <see cref="FName"/> type.
        /// </summary>
        /// <param name="other">The <see cref="FName"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the current instance is equal to the specified <paramref name="other"/> instance;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Equals(FName other)
        {
            return id == other.id;
        }

        /// <summary>
        /// Compares the current instance with another <see cref="FName"/> object and returns an integer  that indicates
        /// whether the current instance precedes, follows, or occurs in the same position  in the sort order as the
        /// other object.
        /// </summary>
        /// <param name="other">The <see cref="FName"/> object to compare to the current instance.</param>
        /// <returns>A signed integer that indicates the relative order of the objects being compared: <list type="bullet">
        /// <item> <description>Less than zero: The current instance precedes <paramref name="other"/> in the sort
        /// order.</description> </item> <item> <description>Zero: The current instance occurs in the same position in
        /// the sort order as <paramref name="other"/>.</description> </item> <item> <description>Greater than zero: The
        /// current instance follows <paramref name="other"/> in the sort order.</description> </item> </list></returns>
        public int CompareTo(FName other)
        {
            return id.CompareTo(other.id);
        }
        #endregion

        #region Operators
        /// <summary>
        /// Implicitly converts a string to an <see cref="FName"/> instance.
        /// </summary>
        /// <param name="value">The string value to convert to an <see cref="FName"/>.</param>
        public static implicit operator FName(string value)
        {
            return new FName(pool.Register(value));
        }

        /// <summary>
        /// Determines whether two <see cref="FName"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="FName"/> instance to compare.</param>
        /// <param name="right">The second <see cref="FName"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="FName"/> instances are equal; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator ==(FName left, FName right)
        {
            return left.id == right.id;
        }

        /// <summary>
        /// Determines whether two <see cref="FName"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="FName"/> instance to compare.</param>
        /// <param name="right">The second <see cref="FName"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="FName"/> instances are not equal; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator !=(FName left, FName right)
        {
            return left.id != right.id;
        }
        #endregion

        #region Nested Types
        /// <summary>
        /// Represents a pool for managing unique string-to-integer mappings and their reverse lookups.
        /// </summary>
        /// <remarks>This class provides functionality to register strings and retrieve their
        /// corresponding unique integer identifiers, as well as to retrieve the original string associated with a given
        /// identifier. Identifiers are assigned sequentially, starting from 1, and the identifier 0 is reserved for an
        /// empty or invalid string.</remarks>
        private class FNamePool
        {
            #region Fields
            private readonly Dictionary<string, int> stringToId = new(StringComparer.Ordinal);

            private readonly List<string> idToString = new() { string.Empty };
            #endregion

            #region Public Methods
            public int Register(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return 0;
                }

                if (stringToId.TryGetValue(value, out int existingId))
                {
                    return existingId;
                }

                int newId = idToString.Count;

                stringToId.Add(value, newId);

                idToString.Add(value);

                return newId;
            }

            public string GetString(int id)
            {
                if (id <= 0 || id >= idToString.Count)
                {
                    return string.Empty;
                }

                return idToString[id];
            }
            #endregion
        }
        #endregion
    }
}