using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Roaring.Net.Benchmarks.Columns;
using Roaring.Net.CRoaring;

#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

#if RELEASE
using BenchmarkDotNet.Diagnostics.Windows.Configs;
#endif

namespace Roaring.Net.Benchmarks;

[ShortRunJob(RuntimeMoniker.Net80)]
[ShortRunJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
#if RELEASE
[NativeMemoryProfiler]
#endif
[Config(typeof(Config))]
public class AllocationBenchmark
{
    private List<uint[]> _values = default!;
    private List<Roaring32Bitmap> _roaring32Bitmap = default!;
    private List<HashSet<uint>> _hashSet = default!;

    private class Config : ManualConfig
    {
        public Config()
        {
            AddColumn(new TotalAllocatedColumn());
            AddColumn(new TotalAllocatedRatioColumn());
        }
    }

    [GlobalSetup]
    public void Setup()
    {
        // https://github.com/dotnet/BenchmarkDotNet/issues/1738
        _values = TestFileLoader.GetValuesForPath("TestData/census-income").GetAwaiter().GetResult();
        _roaring32Bitmap = _values.Select(x => new Roaring32Bitmap(x)).ToList();
        _roaring32Bitmap.ForEach(x => x.Optimize());
        _roaring32Bitmap.ForEach(x => x.ShrinkToFit());
        _hashSet = _values.Select(x => new HashSet<uint>(x)).ToList();
    }

    [Benchmark]
    public int Roaring32()
    {
        var roaring32Bitmap = _values.Select(x => new Roaring32Bitmap(x)).ToList();

        var count = roaring32Bitmap.Count;

        roaring32Bitmap.ForEach(x => x.Dispose());

        return count;
    }

    [Benchmark(Baseline = true)]
    public int OptimizedRoaring32()
    {
        var roaring32Bitmap = _roaring32Bitmap.Select(x => x.Clone()).ToList();

        var count = roaring32Bitmap.Count;

        roaring32Bitmap.ForEach(x => x.Dispose());

        return count;
    }

    [Benchmark]
    public int FrozenRoaring32()
    {
        var roaring32Bitmap = _roaring32Bitmap.Select(x => x.ToFrozen()).ToList();

        var count = roaring32Bitmap.Count;

        roaring32Bitmap.ForEach(x => x.Dispose());

        return count;
    }

    [Benchmark]
    public int HashSet()
    {
        var hashSet = _values.Select(x => new HashSet<uint>(x)).ToList();
        return hashSet.Count;
    }

    [Benchmark]
    public int FrozenSet()
    {
#if NET8_0_OR_GREATER
        var hashSet = _hashSet.Select(x => x.ToFrozenSet()).ToList();
        return hashSet.Count;
#else
        throw new NotSupportedException();
#endif
    }

    [Benchmark]
    public int Array()
    {
        var values = _values.Select(x =>
        {
            var valueArray = new uint[x.Length];
            x.CopyTo(valueArray.AsSpan());
            return valueArray;
        }).ToList();
        return values.Count;
    }
}