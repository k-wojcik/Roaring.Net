using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Roaring.Net.Tests.CRoaring;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class InlineTestObjectAttribute(params object[] data) : DataAttribute
{
    private static readonly object[] FactoriesObjects = TestObjectFactories.Instances.Cast<object>().ToArray().ToArray();

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        => GetMatrix(testMethod).Select(item => data.Append(item).ToArray());

    private static object[] GetMatrix(MethodInfo testMethod)
    {
        ParameterInfo parameter = testMethod.GetParameters().Last();
        return parameter.ParameterType switch
        {
            _ when parameter.ParameterType == typeof(IRoaring32BitmapTestObjectFactory)
                => FactoriesObjects,
            _ => throw new InvalidOperationException("Not supported parameter type")
        };
    }
}