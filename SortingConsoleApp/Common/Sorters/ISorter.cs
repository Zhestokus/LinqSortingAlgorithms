using System.Collections.Generic;

namespace SortingConsoleApp.Common.Sorters
{
    public interface ISorter<TElement>
    {
        IEnumerable<TElement> Sort(IEnumerable<TElement> collection);
        IEnumerable<TElement> Sort(IEnumerable<TElement> collection, bool parallel);

        IEnumerable<TElement> Sort(IEnumerable<TElement> collection, IComparer<TElement> comparer);
        IEnumerable<TElement> Sort(IEnumerable<TElement> collection, IComparer<TElement> comparer, bool parallel);
    }
}
