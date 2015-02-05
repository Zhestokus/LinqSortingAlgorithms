using System;
using System.Collections.Generic;

namespace SortingConsoleApp.Common.Sorters
{
    public abstract class SorterBase<TElement> : ISorter<TElement>
    {
        protected const int Threshold = 2048;

        private static readonly Lazy<ISorter<TElement>> _instance = new Lazy<ISorter<TElement>>(() => new MergesortSorter<TElement>());

        public static ISorter<TElement> Default
        {
            get { return _instance.Value; }
        }

        public IEnumerable<TElement> Sort(IEnumerable<TElement> collection)
        {
            var parallel = Environment.ProcessorCount > 1;
            return Sort(collection, parallel);
        }
        public IEnumerable<TElement> Sort(IEnumerable<TElement> collection, bool parallel)
        {
            return Sort(collection, Comparer<TElement>.Default, parallel);
        }

        public IEnumerable<TElement> Sort(IEnumerable<TElement> collection, IComparer<TElement> comparer)
        {
            var parallel = Environment.ProcessorCount > 1;
            return Sort(collection, comparer, parallel);
        }

        public abstract IEnumerable<TElement> Sort(IEnumerable<TElement> collection, IComparer<TElement> comparer, bool parallel);

        protected void Shuffle(TElement[] array, int low, int high)
        {
            if (low < 0 || low > high || high >= array.Length)
                throw new IndexOutOfRangeException("Illegal subarray range");

            var rand = new Random();

            for (int i = low; i <= high; i++)
            {
                var r = i + rand.Next(high - i + 1);     // between i and hi
                var temp = array[i];
                array[i] = array[r];
                array[r] = temp;
            }
        }

        protected void Swap(TElement[] array, int x, int y)
        {
            var element = array[x];
            array[x] = array[y];
            array[y] = element;
        }
    }
}
