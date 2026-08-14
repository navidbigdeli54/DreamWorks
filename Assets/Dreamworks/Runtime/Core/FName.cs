using System;
using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Core
{
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
        private FName(int id)
        {
            this.id = id;
        }

        public FName(string value)
        {
            id = pool.Register(value);
        }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return pool.GetString(id);
        }

        public override int GetHashCode()
        {
            return id;
        }

        public override bool Equals(object obj)
        {
            return obj is FName other && Equals(other);
        }

        public bool Equals(FName other)
        {
            return id == other.id;
        }

        public int CompareTo(FName other)
        {
            return id.CompareTo(other.id);
        }
        #endregion

        #region Operators
        public static implicit operator FName(string value)
        {
            return new FName(pool.Register(value));
        }

        public static bool operator ==(FName left, FName right)
        {
            return left.id == right.id;
        }

        public static bool operator !=(FName left, FName right)
        {
            return left.id != right.id;
        }
        #endregion

        #region Nested Types
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