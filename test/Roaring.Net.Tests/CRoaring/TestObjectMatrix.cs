namespace Roaring.Net.Tests.CRoaring;

public class TestObjectMatrix<TX, TY>(TX x, TY y)
{
    public TX X { get; } = x;
    public TY Y { get; } = y;
}

public class TestObjectMatrix<TX, TY, TZ>(TX x, TY y, TZ z) : TestObjectMatrix<TX, TY>(x, y)
{
    public TZ Z { get; } = z;
}
