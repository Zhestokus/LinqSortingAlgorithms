using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SortingConsoleApp.Common.Sorters
{
    public class MergesortSorter<TElement> : SorterBase<TElement>
    {
        public override IEnumerable<TElement> Sort(IEnumerable<TElement> collection, IComparer<TElement> comparer, bool parallel)
        {
            // Create a copy of the original array. Switching between
            // original array and its copy will allow to avoid
            // additional array allocations and data copying.
            var array = collection.ToArray();

            var copy = (TElement[])array.Clone();
            if (parallel)
                MergesortParallel(array, copy, comparer, 0, array.Length - 1, GetMaxDepth());
            else
                Mergesort(array, copy, comparer, 0, array.Length - 1);

            return copy;
        }

        private void Mergesort(TElement[] to, TElement[] temp, IComparer<TElement> comparer, int low, int high)
        {
            if (low >= high)
                return;
            var mid = (low + high) / 2;
            // On the way down the recursion tree both arrays have
            // the same data so we can switch them. Sort two
            // sub-arrays first so that they are placed into the temp
            // array.
            Mergesort(temp, to, comparer, low, mid);
            Mergesort(temp, to, comparer, mid + 1, high);
            // Once temp array contains two sorted sub-arrays
            // they are merged into target array.
            Merge(to, temp, comparer, low, mid, mid + 1, high, low);
            // On the way up either we are done as the target array
            // is the original array and now contains required
            // sub-array sorted or it is the temp array from previous
            // step and contains smaller sub-array that will be
            // merged into the target array from previous step
            // (which is the temp array of this step and so we
            // can destroy its contents).
        }

        private void Merge(TElement[] to, TElement[] temp, IComparer<TElement> comparer, int lowX, int highX, int lowY, int highY, int lowTo)
        {
            var highTo = lowTo + highX - lowX + highY - lowY + 1;
            for (; lowTo <= highTo; lowTo++)
            {
                if (lowX > highX)
                    to[lowTo] = temp[lowY++];
                else if (lowY > highY)
                    to[lowTo] = temp[lowX++];
                else
                    to[lowTo] = Less(comparer, temp[lowX], temp[lowY]) ? temp[lowX++] : temp[lowY++];
            }
        }

        private void MergesortParallel(TElement[] to, TElement[] temp, IComparer<TElement> comparer, int low, int high, int depth)
        {
            if (high - low + 1 <= Threshold || depth <= 0)
            {
                // Resort to sequential algorithm if either
                // recursion depth limit is reached or sub-problem
                // size is not big enough to solve it in parallel.
                Mergesort(to, temp, comparer, low, high);
                return;
            }

            var mid = (low + high) / 2;
            // The same target/temp arrays switching technique
            // as in sequential version applies in parallel
            // version. sub-arrays are independent and thus can
            // be sorted in parallel.
            depth--;
            Parallel.Invoke(() => MergesortParallel(temp, to, comparer, low, mid, depth), () => MergesortParallel(temp, to, comparer, mid + 1, high, depth));
            // Once both taks ran to completion merge sorted
            // sub-arrays in parallel.
            MergeParallel(to, temp, comparer, low, mid, mid + 1, high, low, depth);
        }

        private void MergeParallel(TElement[] to, TElement[] temp, IComparer<TElement> comparer, int lowX, int highX, int lowY, int highY, int lowTo, int depth)
        {
            var lengthX = highX - lowX + 1;
            var lengthY = highY - lowY + 1;

            if (lengthX + lengthY <= Threshold || depth <= 0)
            {
                // Resort to sequential algorithm in case of small
                // sub-problem or deep recursion.
                Merge(to, temp, comparer, lowX, highX, lowY, highY, lowTo);
                return;
            }

            if (lengthX < lengthY)
            {
                // Make sure that X range no less than Y range and
                // if needed swap them.
                MergeParallel(to, temp, comparer, lowY, highY, lowX, highX, lowTo, depth);
                return;
            }

            // Get median of the X sub-array. As X sub-array is
            // sorted it means that X[lowX .. midX - 1] are less
            // than or equal to median and X[midx + 1 .. highX]
            // are greater or equal to median.
            var midX = (lowX + highX) / 2;
            // Find element in the Y sub-array that is strictly
            // greater than X[midX]. Again as Y sub-array is
            // sorted Y[lowY .. midY - 1] are less than or equal
            // to X[midX] and Y[midY .. highY] are greater than
            // X[midX].
            var midY = BinarySearch(temp, comparer, lowY, highY, temp[midX]);
            // Now we can compute final position in the target
            // array of median of the X sub-array.
            var midTo = lowTo + midX - lowX + midY - lowY;
            to[midTo] = temp[midX];
            // The rest is to merge X[lowX .. midX - 1] with
            // Y[lowY .. midY - 1] and X[midx + 1 .. highX]
            // with Y[midY .. highY] preceeding and following
            // median respectively in the target array. As
            // pairs are idependent from their final position
            // perspective they can be merged in parallel.
            depth--;

            Parallel.Invoke
            (
                () => MergeParallel(to, temp, comparer, lowX, midX - 1, lowY, midY - 1, lowTo, depth),
                () => MergeParallel(to, temp, comparer, midX + 1, highX, midY, highY, midTo + 1, depth)
            );
        }

        private int BinarySearch(TElement[] from, IComparer<TElement> comparer, int low, int high, TElement lessThanOrEqualTo)
        {
            high = Math.Max(low, high + 1);

            while (low < high)
            {
                var mid = (low + high) / 2;
                if (Less(comparer, from[mid], lessThanOrEqualTo))
                    low = mid + 1;
                else
                    high = mid;
            }

            return low;
        }

        private bool Less(IComparer<TElement> comparer, TElement x, TElement y)
        {
            return comparer.Compare(x, y) < 0;
        }

        private int GetMaxDepth()
        {
            // Although at each step we split unsorted array
            // into two equal size sub-arrays sorting them
            // not be perfectly balanced because parallel merge
            // may not be balanced. So we add some extra space for
            // task creation and so will keep CPUs busy.
            return (int)Math.Log(Environment.ProcessorCount, 2) + 4;
        }
    }
}