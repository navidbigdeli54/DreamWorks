using System.Collections;
using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
            {
                return true;
            }

            if (collection is ICollection<T> genericCollection)
            {
                return genericCollection.Count == 0;
            }

            if (collection is IReadOnlyCollection<T> readOnlyCollection)
            {
                return readOnlyCollection.Count == 0;
            }

            if (collection is ICollection collectionWithCount)
            {
                return collectionWithCount.Count == 0;
            }

            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                return enumerator.MoveNext() == false;
            }
        }

        public static bool IsEmpty(this IEnumerable collection)
        {
            if (collection == null)
            {
                return true;
            }

            if (collection is ICollection collectionWithCount)
            {
                return collectionWithCount.Count == 0;
            }

            IEnumerator enumerator = collection.GetEnumerator();

            try
            {
                return enumerator.MoveNext() == false;
            }
            finally
            {
                (enumerator as System.IDisposable)?.Dispose();
            }
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.IsEmpty() == false;
        }

        public static bool IsNotEmpty(this IEnumerable collection)
        {
            return collection != null && collection.IsEmpty() == false;
        }
    }
}