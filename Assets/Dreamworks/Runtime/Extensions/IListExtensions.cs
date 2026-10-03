using UnityEngine;
using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Extensions
{
    public static class IListExtensions
    {
        public static T RandomRange<T>(this IReadOnlyList<T> collection)
        {
            if (collection.Count == 0)
            {
                return default;
            }

            int index = Random.Range(0, collection.Count - 1);

            return collection[index];
        }
    }
}