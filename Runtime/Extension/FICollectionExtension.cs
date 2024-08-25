using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.Extension.Collection
{
	public static class FICollectionExtension
    {
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }

        public static bool IsNotEmpty<T>(this ICollection<T> collection)
        {
            return !collection.IsEmpty();
        }
    }
}