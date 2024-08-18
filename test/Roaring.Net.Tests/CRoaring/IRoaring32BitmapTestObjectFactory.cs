namespace Roaring.Net.Tests.CRoaring;

public interface IRoaring32BitmapTestObjectFactory
{
    IRoaring32BitmapTestObject GetDefault();
    IRoaring32BitmapTestObject GetEmpty();
    IRoaring32BitmapTestObject GetForCount(uint count);
    IRoaring32BitmapTestObject GetFromValues(uint[] values);
    IRoaring32BitmapTestObject GetForRange(uint start, uint end);
    IRoaring32BitmapTestObject GetForRange(uint start, uint end, uint count);
}

public interface IRoaring32BitmapTestObjectFactory<out TTestObject>
    where TTestObject : IRoaring32BitmapTestObject
{
    TTestObject GetDefault();
    TTestObject GetEmpty();
    TTestObject GetForCount(uint count);
    TTestObject GetFromValues(uint[] values);
    TTestObject GetForRange(uint start, uint end);
    TTestObject GetForRange(uint start, uint end, uint count);
}