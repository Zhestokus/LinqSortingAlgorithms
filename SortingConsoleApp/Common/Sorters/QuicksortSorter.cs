using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SortingConsoleApp.Common.Sorters
{
    public class QuicksortSorter<TElement> : SorterBase<TElement>
    {
        public override IEnumerable<TElement> Sort(IEnumerable<TElement> collection, IComparer<TElement> comparer, bool parallel)
        {
            var array = collection.ToArray();

            if (parallel)
                QuicksortParallel(array, comparer, 0, array.Length - 1);
            else
                Quicksort(array, comparer, 0, array.Length - 1);

            return array;
        }

        private void Quicksort(TElement[] array, IComparer<TElement> comparer, int low, int high)
        {
            // If the list contains one or less element: no need to sort!
            if (high <= low) return;

            // Partitioning our list
            var pivot = Partition(array, comparer, low, high);

            // Sorting the left of the pivot
            Quicksort(array, comparer, low, pivot - 1);
            // Sorting the right of the pivot
            Quicksort(array, comparer, pivot + 1, high);
        }

        private void QuicksortParallel(TElement[] array, IComparer<TElement> comparer, int low, int high)
        {
            // If the list to sort contains one or less element, the list is already sorted.
            if (high <= low)
                return;

            // If the size of the list is under the threshold, sequential version is used.
            if (high - low < Threshold)
                Quicksort(array, comparer, low, high);
            else
            {
                // Partitioning our list and getting a pivot.
                var pivot = Partition(array, comparer, low, high);

                // Sorting the left and right of the pivot in parallel
                Parallel.Invoke(() => QuicksortParallel(array, comparer, low, pivot - 1), () => QuicksortParallel(array, comparer, pivot + 1, high));
            }
        }

        private int Partition(TElement[] array, IComparer<TElement> comparer, int low, int high)
        {
            /*
                * Defining the pivot position, here the middle element is used but the choice of a pivot
                * is a rather complicated issue. Choosing the left element brings us to a worst-case performance,
                * which is quite a common case, that's why it's not used here.
                */
            var pivotPos = (high + low) / 2;
            var pivot = array[pivotPos];
            // Putting the pivot on the left of the partition (lowest index) to simplify the loop
            Swap(array, low, pivotPos);

            /** The pivot remains on the lowest index until the end of the loop
                * The left variable is here to keep track of the number of values
                * that were compared as "less than" the pivot.
                */
            var left = low;
            for (var i = low + 1; i <= high; i++)
            {
                // If the value is greater than the pivot value we continue to the next index.
                if (comparer.Compare(array[i], pivot) >= 0)
                    continue;

                // If the value is less than the pivot, we increment our left counter (one more element below the pivot)
                left++;
                // and finally we swap our element on our left index.
                Swap(array, i, left);
            }

            // The pivot is still on the lowest index, we need to put it back after all values found to be "less than" the pivot.
            Swap(array, low, left);

            // We return the new index of our pivot
            return left;
        }
    }
}