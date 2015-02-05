using System.Collections.Generic;

namespace SortingConsoleApp.Common.Utils
{
    public static class SortingUtils
    {
        public static bool IsSorted<TElement>(TElement[] array)
        {
            return IsSorted(array, Comparer<TElement>.Default, 0, array.Length - 1);
        }
        public static bool IsSorted<TElement>(TElement[] array, IComparer<TElement> comparer)
        {
            return IsSorted(array, comparer, 0, array.Length - 1);
        }

        public static bool IsSorted<TElement>(TElement[] array, IComparer<TElement> comparer, int low, int high)
        {
            for (int i = low + 1; i <= high; i++)
                if (Less(array[i], array[i - 1], comparer))
                    return false;

            return true;
        }

        private static bool Less<TElement>(TElement x, TElement y, IComparer<TElement> comparer)
        {
            return (comparer.Compare(x, y) < 0);
        }
    }
}