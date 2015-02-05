using System.Collections.Generic;
using System.Linq;

namespace SortingConsoleApp.Common.Sorters
{
    public class HeapsortSorter<TElement> : SorterBase<TElement>
    {
        public override IEnumerable<TElement> Sort(IEnumerable<TElement> collection, IComparer<TElement> comparer, bool parallel)
        {
            var array = collection.ToArray();

            var len = array.Length;

            for (int k = len / 2; k >= 1; k--)
                Sink(array, comparer, k, len);

            while (len > 1)
            {
                Exch(array, 1, len--);
                Sink(array, comparer, 1, len);
            }

            return array;
        }

        private void Sink(TElement[] array, IComparer<TElement> comparar, int k, int N)
        {
            while (2 * k <= N)
            {
                int j = 2 * k;
                if (j < N && Less(array, comparar, j, j + 1))
                    j++;

                if (!Less(array, comparar, k, j))
                    break;

                Exch(array, k, j);
                k = j;
            }
        }

        private bool Less(TElement[] array, IComparer<TElement> comparar, int i, int j)
        {
            return Less(comparar, array[i - 1], array[j - 1]);
        }

        private void Exch(TElement[] array, int i, int j)
        {
            var swap = array[i - 1];
            array[i - 1] = array[j - 1];
            array[j - 1] = swap;
        }

        private bool Less(IComparer<TElement> comparar, TElement x, TElement y)
        {
            return comparar.Compare(x, y) < 0;
        }
    }
}
