using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) where T: class
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}
