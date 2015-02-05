using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SortingConsoleApp.Common.Sorters
{
    public class QuicksortIndexSorter<TElement> : SorterBase<TElement>
    {
        public override IEnumerable<TElement> Sort(IEnumerable<TElement> collection, IComparer<TElement> comparer, bool parallel)
        {
            var quicksorter = new Quicksorter(collection, comparer, parallel);
            return quicksorter.Sort();
        }

        private class Quicksorter
        {
            private readonly bool _parallel;
            private readonly int[] _indexes;
            private readonly TElement[] _array;
            private readonly IComparer<TElement> _comparer;

            public Quicksorter(IEnumerable<TElement> collection, IComparer<TElement> comparer, bool parallel)
            {
                _comparer = comparer;
                _parallel = parallel;

                _array = collection.ToArray();
                _indexes = new int[_array.Length];

                for (int i = 0; i < _array.Length; i++)
                    _indexes[i] = i;
            }

            public IEnumerable<TElement> Sort()
            {
                if (_parallel)
                    QuicksortParallel(0, _array.Length - 1);
                else
                    Quicksort(0, _array.Length - 1);

                foreach (var index in _indexes)
                    yield return _array[index];
            }

            private void Quicksort(int low, int high)
            {
                // If the list contains one or less element: no need to sort!
                if (high <= low) 
                    return;

                // Partitioning our list
                var pivot = Partition(low, high);

                // Sorting the left of the pivot
                Quicksort(low, pivot - 1);
                // Sorting the right of the pivot
                Quicksort(pivot + 1, high);
            }

            private void QuicksortParallel(int low, int high)
            {
                // If the list to sort contains one or less element, the list is already sorted.
                if (high <= low)
                    return;

                // If the size of the list is under the threshold, sequential version is used.
                if (high - low < Threshold)
                    Quicksort(low, high);
                else
                {
                    // Partitioning our list and getting a pivot.
                    var pivot = Partition(low, high);

                    // Sorting the left and right of the pivot in parallel
                    Parallel.Invoke(() => QuicksortParallel(low, pivot - 1), () => QuicksortParallel(pivot + 1, high));
                }
            }

            private int Partition(int low, int high)
            {
                /*
                    * Defining the pivot position, here the middle element is used but the choice of a pivot
                    * is a rather complicated issue. Choosing the left element brings us to a worst-case performance,
                    * which is quite a common case, that's why it's not used here.
                    */
                var pivotPos = (high + low) / 2;
                var pivot = _indexes[pivotPos];
                // Putting the pivot on the left of the partition (lowest index) to simplify the loop
                Swap(low, pivotPos);

                /** The pivot remains on the lowest index until the end of the loop
                    * The left variable is here to keep track of the number of values
                    * that were compared as "less than" the pivot.
                    */
                var left = low;
                for (var i = low + 1; i <= high; i++)
                {
                    // If the value is greater than the pivot value we continue to the next index.
                    if (CompareItems(_indexes[i], pivot) >= 0)
                        continue;

                    // If the value is less than the pivot, we increment our left counter (one more element below the pivot)
                    left++;
                    // and finally we swap our element on our left index.
                    Swap(i, left);
                }

                // The pivot is still on the lowest index, we need to put it back after all values found to be "less than" the pivot.
                Swap(low, left);

                // We return the new index of our pivot
                return left;
            }

            private int CompareItems(int x, int y)
            {
                return _comparer.Compare(_array[x], _array[y]);
            }

            private void Swap(int x, int y)
            {
                var element = _indexes[x];
                _indexes[x] = _indexes[y];
                _indexes[y] = element;
            }
        }
    }
}