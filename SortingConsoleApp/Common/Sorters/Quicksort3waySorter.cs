using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SortingConsoleApp.Common.Sorters
{
    public class Quicksort3waySorter<TElement> : SorterBase<TElement>
    {
        public override IEnumerable<TElement> Sort(IEnumerable<TElement> collection, IComparer<TElement> comparer, bool parallel)
        {
            var array = collection.ToArray();

            Shuffle(array, 0, array.Length - 1);

            if (parallel)
                QuicksortParallel(array, comparer, 0, array.Length - 1);
            else
                Quicksort(array, comparer, 0, array.Length - 1);

            return array;
        }

        private void Quicksort(TElement[] array, IComparer<TElement> comparer, int low, int high)
        {
            if (high <= low)
                return;

            int lt = low;
            int gt = high;

            int i = low;
            var v = array[low];

            while (i <= gt)
            {
                int order = comparer.Compare(array[i], v);
                if (order < 0)
                    Swap(array, lt++, i++);
                else if (order > 0)
                    Swap(array, i, gt--);
                else
                    i++;
            }

            Quicksort(array, comparer, low, lt - 1);
            Quicksort(array, comparer, gt + 1, high);
        }

        private void QuicksortParallel(TElement[] array, IComparer<TElement> comparer, int low, int high)
        {
            if (high <= low)
                return;

            int lt = low;
            int gt = high;

            int i = low;
            var v = array[low];

            while (i <= gt)
            {
                var order = comparer.Compare(array[i], v);
                if (order < 0)
                    Swap(array, lt++, i++);
                else if (order > 0)
                    Swap(array, i, gt--);
                else
                    i++;
            }

            if (high - low < Threshold)
                Quicksort(array, comparer, low, high);
            else
            {
                Parallel.Invoke
                (
                    () => QuicksortParallel(array, comparer, low, lt - 1),
                    () => QuicksortParallel(array, comparer, gt + 1, high)
                );
            }
        }
    }
}