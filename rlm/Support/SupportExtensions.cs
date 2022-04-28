using System;
using System.Collections.Generic;
using System.Linq;

namespace rlm.Support
{
    static class SupportExtensions
    {
        public static T Next<T>(this Random rng, IEnumerable<T> list) =>
            list.ElementAt(rng.Next(list.Count()));

        public static int Next(this Random rng, Range range) =>
            rng.Next(range.Start.Value, range.End.Value);

        public static IEnumerable<T> Next<T>(this Random rng, IEnumerable<T> list, Range sampleSize) =>
            rng.Next(list, rng.Next(sampleSize));

        public static IEnumerable<T> Next<T>(this Random rng, IEnumerable<T> list, int sampleSize)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            //if (sampleSize > list.Count) throw new ArgumentException("sampleSize may not be greater than list count", nameof(sampleSize));

            var indices = new Dictionary<int, int>();

            for (int i = 0; i < sampleSize; i++)
            {
                int j = rng.Next(i, list.Count());
                if (!indices.TryGetValue(j, out int index)) index = j;

                yield return list.ElementAt(index);

                if (!indices.TryGetValue(i, out index)) index = i;
                indices[j] = index;
            }
        }

        public static void SetElements<T>(this IList<T> list, Func<int, T> gen)
        {
            for (int i = 0; i < list.Count; ++i)
                list[i] = gen(i);
        }
    }
}
