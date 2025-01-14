namespace Roaring.Net.Tests.CRoaring;

public interface IRoaring64BitmapTestObjectFactory
{
    IRoaring64BitmapTestObject GetDefault();
    IRoaring64BitmapTestObject GetEmpty();
    IRoaring64BitmapTestObject GetForCount(ulong count);
    IRoaring64BitmapTestObject GetFromValues(ulong[] values);
    IRoaring64BitmapTestObject GetForRange(ulong start, ulong end);
    IRoaring64BitmapTestObject GetForRange(ulong start, ulong end, ulong count);
}

public interface IRoaring64BitmapTestObjectFactory<out TTestObject>
    where TTestObject : IRoaring64BitmapTestObject
{
    TTestObject GetDefault();
    TTestObject GetEmpty();
    TTestObject GetForCount(ulong count);
    TTestObject GetFromValues(ulong[] values);
    TTestObject GetForRange(ulong start, ulong end);
    TTestObject GetForRange(ulong start, ulong end, ulong count);
}