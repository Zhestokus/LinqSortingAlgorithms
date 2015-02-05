using System;
using System.Collections.Generic;

namespace SortingConsoleApp.Common.Comparers
{
    public class SorterComparer<TElement, TKey> : ISorterComparer<TElement>
    {
        public SorterComparer(IComparer<TKey> comparer, Func<TElement, TKey> keySelector, bool descending)
        {
            Comparer = comparer;
            Descending = descending;

            KeySelector = keySelector;
        }

        public bool Descending { get; set; }

        public IComparer<TKey> Comparer { get; set; }

        public Func<TElement, TKey> KeySelector { get; set; }

        public ISorterComparer<TElement> Next { get; set; }

        public int Compare(TElement x, TElement y)
        {
            var xKey = KeySelector(x);
            var yKey = KeySelector(y);

            var order = Comparer.Compare(xKey, yKey);
            if (order == 0 && Next != null)
            {
                order = Next.Compare(x, y);
            }

            if (Descending)
                order = -order;

            return order;
        }
    }
}