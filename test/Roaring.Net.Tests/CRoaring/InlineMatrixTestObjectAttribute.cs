using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Roaring.Net.Tests.CRoaring.FrozenRoaring32BitmapTests;
using Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;
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
    
    private static readonly TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory>[] 
        Matrix3x3 = Matrix2x2.SelectMany(factory => TestObjectFactories.Instances.Select(factoryZ => 
                    new TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory>(factory.X, factory.Y, factoryZ))
                .ToList())
            .ToArray();
    
    private static readonly object[] Matrix3x3Objects = Matrix3x3.Cast<object>().ToArray();
    
    public override IEnumerable<object[]> GetData(MethodInfo testMethod) 
        => GetMatrix(testMethod).Select(item => data.Append(item).ToArray());

    private static object[] GetMatrix(MethodInfo testMethod)
    {
        var parameter = testMethod.GetParameters().Last();
        return parameter.ParameterType switch
        {
            _ when parameter.ParameterType == typeof(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory>) 
                => Matrix2x2Objects,
            _ when parameter.ParameterType == typeof(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory>) 
                => Matrix3x3Objects,
            _ => throw new InvalidOperationException("Not supported parameter type")
        };
    }
}