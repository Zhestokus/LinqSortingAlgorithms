using System.Collections.Generic;

namespace SortingConsoleApp.Common.Comparers
{
    public interface ISorterComparer<TElement> : IComparer<TElement>
    {
        ISorterComparer<TElement> Next { get; set; }
    }
}