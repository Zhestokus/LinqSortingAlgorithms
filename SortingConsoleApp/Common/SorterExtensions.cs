using System;
using System.Collections.Generic;
using System.Linq;
using SortingConsoleApp.Common.Comparers;
using SortingConsoleApp.Common.Sorters;

namespace SortingConsoleApp.Common
{
    public static class SorterExtensions
    {
        #region Sequental

        public static ISorterEnumerable<TElement> SortBy<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
        {
            return SortBy(source, keySelector, Comparer<TKey>.Default);
        }
        public static ISorterEnumerable<TElement> SortBy<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
        {
            return SortBy(source, keySelector, comparer, SorterBase<TElement>.Default);
        }

        public static ISorterEnumerable<TElement> SortBy<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector, ISorter<TElement> sorter)
        {
            return SortBy(source, keySelector, Comparer<TKey>.Default, sorter);
        }
        public static ISorterEnumerable<TElement> SortBy<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, ISorter<TElement> sorter)
        {
            var sorterComparer = new SorterComparer<TElement, TKey>(comparer, keySelector, false);
            return new SorterEnumerable<TElement>(source, sorterComparer, sorter, false);
        }

        public static ISorterEnumerable<TElement> SortByDescending<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
        {
            return SortByDescending(source, keySelector, Comparer<TKey>.Default);
        }
        public static ISorterEnumerable<TElement> SortByDescending<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
        {
            return SortByDescending(source, keySelector, comparer, SorterBase<TElement>.Default);
        }

        public static ISorterEnumerable<TElement> SortByDescending<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector, ISorter<TElement> sorter)
        {
            return SortByDescending(source, keySelector, Comparer<TKey>.Default, sorter);
        }
        public static ISorterEnumerable<TElement> SortByDescending<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, ISorter<TElement> sorter)
        {
            var sorterComparer = new SorterComparer<TElement, TKey>(comparer, keySelector, true);
            return new SorterEnumerable<TElement>(source, sorterComparer, sorter, false);
        }



        #endregion

        #region Parallel

        public static ISorterEnumerable<TElement> SortBy<TElement, TKey>(this ParallelQuery<TElement> source, Func<TElement, TKey> keySelector)
        {
            return SortBy(source, keySelector, Comparer<TKey>.Default);
        }
        public static ISorterEnumerable<TElement> SortBy<TElement, TKey>(this ParallelQuery<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
        {
            return SortBy(source, keySelector, comparer, SorterBase<TElement>.Default);
        }

        public static ISorterEnumerable<TElement> SortBy<TElement, TKey>(this ParallelQuery<TElement> source, Func<TElement, TKey> keySelector, ISorter<TElement> sorter)
        {
            return SortBy(source, keySelector, Comparer<TKey>.Default, sorter);
        }
        public static ISorterEnumerable<TElement> SortBy<TElement, TKey>(this ParallelQuery<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, ISorter<TElement> sorter)
        {
            var sorterComparer = new SorterComparer<TElement, TKey>(comparer, keySelector, false);
            return new SorterEnumerable<TElement>(source, sorterComparer, sorter, true);
        }

        public static ISorterEnumerable<TElement> SortByDescending<TElement, TKey>(this ParallelQuery<TElement> source, Func<TElement, TKey> keySelector)
        {
            return SortByDescending(source, keySelector, Comparer<TKey>.Default);
        }
        public static ISorterEnumerable<TElement> SortByDescending<TElement, TKey>(this ParallelQuery<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
        {
            return SortByDescending(source, keySelector, comparer, SorterBase<TElement>.Default);
        }

        public static ISorterEnumerable<TElement> SortByDescending<TElement, TKey>(this ParallelQuery<TElement> source, Func<TElement, TKey> keySelector, ISorter<TElement> sorter)
        {
            return SortByDescending(source, keySelector, Comparer<TKey>.Default, sorter);
        }
        public static ISorterEnumerable<TElement> SortByDescending<TElement, TKey>(this ParallelQuery<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, ISorter<TElement> sorter)
        {
            var sorterComparer = new SorterComparer<TElement, TKey>(comparer, keySelector, true);
            return new SorterEnumerable<TElement>(source, sorterComparer, sorter, true);
        }

        #endregion

        #region Common

        public static ISorterEnumerable<TElement> ThenBy<TElement, TKey>(this ISorterEnumerable<TElement> source, Func<TElement, TKey> keySelector)
        {
            return ThenBy(source, keySelector, Comparer<TKey>.Default);
        }
        public static ISorterEnumerable<TElement> ThenBy<TElement, TKey>(this ISorterEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
        {
            var sourceComparer = source.Comparer;

            var nextComparer = new SorterComparer<TElement, TKey>(comparer, keySelector, false);
            sourceComparer.Next = nextComparer;

            return source;
        }

        public static ISorterEnumerable<TElement> ThenByDescending<TElement, TKey>(this ISorterEnumerable<TElement> source, Func<TElement, TKey> keySelector)
        {
            return ThenByDescending(source, keySelector, Comparer<TKey>.Default);
        }
        public static ISorterEnumerable<TElement> ThenByDescending<TElement, TKey>(this ISorterEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
        {
            var sourceComparer = source.Comparer;

            var nextComparer = new SorterComparer<TElement, TKey>(comparer, keySelector, true);
            sourceComparer.Next = nextComparer;

            return source;
        }

        #endregion
    }
}
