using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Extensions
{
    public static class ICollectionExtensions
    {
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            if (collection == null)
            {
                return true;
            }

            return collection.Count == 0;
        }

        public static bool IsEmpty<T>(this IReadOnlyCollection<T> collection)
        {
            if (collection == null)
            {
                return true;
            }

            return collection.Count == 0;
        }

        public static bool IsNotEmpty<T>(this ICollection<T> collection)
        {
            return collection.IsEmpty() == false;
        }

        public static bool IsNotEmpty<T>(this IReadOnlyCollection<T> collection)
        {
            return collection.IsEmpty() == false;
        }

        public static bool AddUnique<T>(this ICollection<T> collection, T item)
        {
            if (!collection.Contains(item))
            {
                collection.Add(item);

                return true;
            }

            return false;
        }
    }
}