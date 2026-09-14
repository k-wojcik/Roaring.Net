using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Internal;
using Xunit.Sdk;
using Xunit.v3;

namespace Roaring.Net.Tests.CRoaring;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class InlineTestObjectAttribute(params object[] data) : DataAttribute
{
    private static readonly object[] FactoriesObjects = TestObjectFactories.Instances.Cast<object>().ToArray().ToArray();
    private static readonly object[] FactoriesFor64BitObjects = TestObjectFactories.InstancesFor64Bit.Cast<object>().ToArray().ToArray();

    public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod, DisposalTracker disposalTracker)
        => ValueTask.FromResult(
            (IReadOnlyCollection<ITheoryDataRow>)
            [
                .. GetMatrix(testMethod)
                    .Select(item => data.Append(item).ToArray())
                    .Select(ITheoryDataRow (row) => new TheoryDataRow(row))
            ]);

    public override bool SupportsDiscoveryEnumeration()
        => true;

    private static object[] GetMatrix(MethodInfo testMethod)
    {
        ParameterInfo parameter = testMethod.GetParameters().Last();
        return parameter.ParameterType switch
        {
            _ when parameter.ParameterType == typeof(IRoaring32BitmapTestObjectFactory)
                => FactoriesObjects,
            _ when parameter.ParameterType == typeof(IRoaring64BitmapTestObjectFactory)
                => FactoriesFor64BitObjects,
            _ => throw new InvalidOperationException("Not supported parameter type")
        };
    }
}