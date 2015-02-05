using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Solution
{
    static void Main(String[] args)
    {
        var x = "125875";
        var y = "251748";

        var xSet = new HashSet<char>(x);
        var ySet = new HashSet<char>(y);

        Console.WriteLine(xSet.SetEquals(ySet));
        Console.ReadKey();
    }
}