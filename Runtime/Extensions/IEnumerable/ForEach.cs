using System;
using System.Collections.Generic;

namespace UniCtor.Extensions.IEnumerable
{
    internal static partial class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action.Invoke(item);

            return source;
        }
    }
}