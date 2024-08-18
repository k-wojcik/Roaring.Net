using System.Collections.Generic;
using Roaring.Net.Tests.CRoaring.FrozenRoaring32BitmapTests;
using Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

namespace Roaring.Net.Tests.CRoaring;

internal static class TestObjectFactories
{
    public static readonly List<IRoaring32BitmapTestObjectFactory> Instances = new()
    {
        new Roaring32BitmapTestObjectFactory(),
        new FrozenRoaring32BitmapTestObjectFactory(),
    };
}