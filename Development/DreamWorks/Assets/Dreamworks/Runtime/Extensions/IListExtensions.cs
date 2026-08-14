using System.Collections.Generic;
using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Extensions
{
    public static class IListExtensions
    {
        public static T RandomRange<T>(this IList<T> collection)
        {
            if (collection.IsEmpty())
            {
                return default;
            }

            int index = Random.Range(0, collection.Count - 1);

            return collection[index];
        }

        public static T RandomRange<T>(this IReadOnlyList<T> collection)
        {
            if (collection.IsEmpty())
            {
                return default;
            }

            int index = Random.Range(0, collection.Count - 1);

            return collection[index];
        }
    }
}