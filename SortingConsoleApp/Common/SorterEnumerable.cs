using System.Collections;
using System.Collections.Generic;
using SortingConsoleApp.Common.Comparers;
using SortingConsoleApp.Common.Sorters;

namespace SortingConsoleApp.Common
{
    public class SorterEnumerable<TElement> : ISorterEnumerable<TElement>
    {
        private readonly IEnumerable<TElement> _source;

        public SorterEnumerable(IEnumerable<TElement> source, ISorterComparer<TElement> comparer, ISorter<TElement> sorter, bool parallel)
        {
            _source = source;

            Parallel = parallel;
            Comparer = comparer;
            Sorter = sorter;
        }

        public bool Parallel { get; set; }

        public ISorter<TElement> Sorter { get; private set; }

        public ISorterComparer<TElement> Comparer { get; private set; }

        public IEnumerator<TElement> GetEnumerator()
        {
            var sorted = Sorter.Sort(_source, Comparer, Parallel);

            foreach (var element in sorted)
                yield return element;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}