using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineSumator.Tests.Helpers
{
    public static class Enumerable
    {
        public static bool AreEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) where T : IComparable<T>
        {
            if (actual is null)
            {
                throw new ArgumentNullException(nameof(actual));
            }

            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected));
            }

            if (actual.TryGetNonEnumeratedCount(out var actualCount) && expected.TryGetNonEnumeratedCount(out var expectedCount) && actualCount != expectedCount)
            {
                return false;
            }

            using var actualEnum = actual.GetEnumerator();
            using var expectedEnum = expected.GetEnumerator();

            while (true)
            {
                var actualMoveNext = actualEnum.MoveNext();
                var expectedMoveNext = expectedEnum.MoveNext();

                if (!actualMoveNext && !expectedMoveNext)
                {
                    return true;
                }

                if (actualMoveNext != expectedMoveNext || actualEnum.Current.CompareTo(expectedEnum.Current) != 0)
                {
                    return false;
                }
            }
        }
    }
}
