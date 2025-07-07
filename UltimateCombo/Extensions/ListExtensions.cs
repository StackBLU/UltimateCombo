using System;
using System.Collections.Generic;

namespace UltimateCombo.Extensions
{
    internal static class ListExtensions
    {
        private static readonly Random RNG = new();

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = RNG.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}
