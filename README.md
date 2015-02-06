# LinqSortingAlgorithms

Provides more powerfull extensions for sorting collection via LINQ (like OrderBy method)

it supports single threaded and multithreaded sorting mode (e.g  single threaded - list.SortBy(...), multithreaded list.AsParallel().SortBy(...))
also you can use other sorting algorithms like Mergesort, Quicksort3way, Heapsort, Treesort or write implementation of any other 
algorithm with ISorter interface and then use it with SortBy() extension method
