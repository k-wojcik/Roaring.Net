using System;
using System.Collections.Generic;

namespace Roaring.Net.Tests.Extensions;

internal static class EnumerableRange
{
    public static IEnumerable<ulong> Range(ulong start, ulong count)
    {
        ulong max = start + count - 1;
        if (start > max)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        if (count == 0)
        {
            yield break;
        }

        for (ulong i = start; i <= max && i >= start; i++)
        {
            yield return i;
        }
    }
}