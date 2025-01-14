using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Roaring.Net.Tests.CRoaring;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
internal sealed class InlineMatrixTestObjectAttribute(params object[] data) : DataAttribute
{
    private static readonly TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory>[]
        Matrix2x2 = TestObjectFactories.Instances.SelectMany(factoryX => TestObjectFactories.Instances.Select(factoryY =>
            new TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory>(factoryX, factoryY))
            .ToList())
            .ToArray();

    private static readonly object[] Matrix2x2Objects = Matrix2x2.Cast<object>().ToArray();

    private static readonly TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory>[]
        Matrix2x2For64Bit = TestObjectFactories.InstancesFor64Bit.SelectMany(factoryX => TestObjectFactories.InstancesFor64Bit.Select(factoryY =>
                    new TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory>(factoryX, factoryY))
                .ToList())
            .ToArray();

    private static readonly object[] Matrix2x2ObjectsFor64Bit = Matrix2x2For64Bit.Cast<object>().ToArray();

    private static readonly TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory>[]
        Matrix3x3 = Matrix2x2.SelectMany(factory => TestObjectFactories.Instances.Select(factoryZ =>
                    new TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory>(factory.X, factory.Y, factoryZ))
                .ToList())
            .ToArray();

    private static readonly object[] Matrix3x3Objects = Matrix3x3.Cast<object>().ToArray();

    private static readonly TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory>[]
        Matrix3x3For64Bit = Matrix2x2For64Bit.SelectMany(factory => TestObjectFactories.InstancesFor64Bit.Select(factoryZ =>
                    new TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory>(factory.X, factory.Y, factoryZ))
                .ToList())
            .ToArray();

    private static readonly object[] Matrix3x3ObjectsFor64Bit = Matrix3x3For64Bit.Cast<object>().ToArray();

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        => GetMatrix(testMethod).Select(item => data.Append(item).ToArray());

    private static object[] GetMatrix(MethodInfo testMethod)
    {
        ParameterInfo parameter = testMethod.GetParameters().Last();
        return parameter.ParameterType switch
        {
            _ when parameter.ParameterType == typeof(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory>)
                => Matrix2x2Objects,
            _ when parameter.ParameterType == typeof(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory>)
                => Matrix3x3Objects,
            _ when parameter.ParameterType == typeof(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory>)
                => Matrix2x2ObjectsFor64Bit,
            _ when parameter.ParameterType == typeof(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory>)
                => Matrix3x3ObjectsFor64Bit,
            _ => throw new InvalidOperationException("Not supported parameter type")
        };
    }
}