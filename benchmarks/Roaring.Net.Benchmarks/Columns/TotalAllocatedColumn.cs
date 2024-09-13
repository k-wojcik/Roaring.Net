using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Metrology;

namespace Roaring.Net.Benchmarks.Columns;

internal class TotalAllocatedColumn : IColumn
{
    public string Id => nameof(TotalAllocatedColumn);
    public string ColumnName => "Total Allocated";

    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase) => GetValue(summary, benchmarkCase, SummaryStyle.Default);

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
    {
        BenchmarkReport report = summary.Reports
            .Single(x => x.BenchmarkCase == benchmarkCase);

        List<KeyValuePair<string, Metric>> metrics = report
            .Metrics
            .Where(x => x.Key is "AllocatedNativeMemoryDescriptor" or "Allocated Memory")
            .ToList();

        double value = metrics.Sum(x => x.Value.Value);

        UnitPresentation unitPresentation = new UnitPresentation(style.PrintUnitsInContent, minUnitWidth: 0, gap: true);
        return SizeValue.FromBytes((long)value).ToString(style.SizeUnit, "0.##", summary.GetCultureInfo(), unitPresentation);
    }

    public bool IsAvailable(Summary summary)
        => summary.Reports
            .SelectMany(x => x.Metrics)
            .Any(x => x.Key is "AllocatedNativeMemoryDescriptor" or "Allocated Memory");

    public bool AlwaysShow => true;

    public ColumnCategory Category => ColumnCategory.Metric;

    public int PriorityInCategory => int.MaxValue - 100;

    public bool IsNumeric => true;

    public UnitType UnitType => UnitType.Size;

    public string Legend => "Total allocated memory (managed + native)";

    public override string ToString() => ColumnName;
}