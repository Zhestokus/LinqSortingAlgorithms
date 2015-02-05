using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SortingConsoleApp.Common;
using SortingConsoleApp.Common.Sorters;

namespace SortingConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var @set = new SortedSet<Guid>();

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 2000000; i++)
            {
                var g = Guid.NewGuid();

                sw.Start();
                @set.Add(g);
                sw.Stop();

            }

            var l = new List<Guid>(@set);
            var query = l.AsQueryable();
            var query1 = l.AsQueryable();
            //var sw1 = Stopwatch.StartNew();
            //l.SortBy(n => n).ToList();
            //sw1.Stop();


            //var sw2 = Stopwatch.StartNew();
            //l.AsParallel().SortBy(n => n).ToList();
            //sw2.Stop();


            var ss1 = Stopwatch.StartNew();
            var dt1 = query.ToArray();
            ss1.Stop();

            var ss2 = Stopwatch.StartNew();
            var dt2 = query1.AsParallel().ToArray();
            ss2.Stop();

            Console.ReadLine();
            //var heapsortSorter = new HeapsortSorter<Guid>();
            //var mergesortSorter = new MergesortSorter<Guid>();
            //var quicksortSorter = new QuicksortSorter<Guid>();
            //var quicksort3waySorter = new Quicksort3waySorter<Guid>();
            //var quicksortIndexSorter = new QuicksortIndexSorter<Guid>();
            //var rbTreeSorter = new RbTreeSorter<Guid>();
            //var rbTreeIndexSorter = new RbTreeIndexSorter<Guid>();

            //var sw1 = Stopwatch.StartNew();
            //heapsortSorter.Sort(l, false);
            //sw1.Stop();

            //var sw2 = Stopwatch.StartNew();
            //mergesortSorter.Sort(l, false);
            //sw2.Stop();

            //var sw3 = Stopwatch.StartNew();
            //quicksortSorter.Sort(l, false);
            //sw3.Stop();

            //var sw4 = Stopwatch.StartNew();
            //quicksort3waySorter.Sort(l, false);
            //sw4.Stop();

            //var sw5 = Stopwatch.StartNew();
            //var dd = quicksortIndexSorter.Sort(l, false).GetEnumerator().MoveNext();
            //sw5.Stop();

            //var sw6 = Stopwatch.StartNew();
            //rbTreeSorter.Sort(l, false);
            //sw6.Stop();

            //var sw7 = Stopwatch.StartNew();
            //rbTreeIndexSorter.Sort(l, false);
            //sw7.Stop();
        }
    }
}
