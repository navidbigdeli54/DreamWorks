using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.Extension.ReadOnlyCollection
{
	public static class FIReadOnlyCollectionExtension
    {
		public static bool IsEmpty<T>(this IReadOnlyCollection<T> collection)
		{
			return collection.Count == 0;
		}

		public static bool IsNotEmpty<T>(this IReadOnlyCollection<T> collection)
		{
			return !collection.IsEmpty();
		}
	}
}