using System.Collections.Generic;
using SortingConsoleApp.Common.Comparers;
using SortingConsoleApp.Common.Sorters;

namespace SortingConsoleApp.Common
{
    public interface ISorterEnumerable<TElement> : IEnumerable<TElement>
    {
        bool Parallel { get; }

        ISorter<TElement> Sorter { get; }

        ISorterComparer<TElement> Comparer { get; }
    }
}