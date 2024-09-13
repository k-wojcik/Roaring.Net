using System.Globalization;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Roaring.Net.Benchmarks.Columns;

internal class TotalAllocatedRatioColumn : BaselineCustomColumn
{
    public override string Id => nameof(TotalAllocatedRatioColumn);
    public override string ColumnName => "Total Allocated Ratio";

    public override string GetValue(Summary summary, BenchmarkCase benchmarkCase, Statistics baseline, IReadOnlyDictionary<string, Metric> baselineMetrics,
        Statistics current, IReadOnlyDictionary<string, Metric> currentMetrics, bool isBaseline)
    {
        double? ratio = GetAllocationRatio(currentMetrics, baselineMetrics);
        double? invertedRatio = GetAllocationRatio(baselineMetrics, currentMetrics);

        if (ratio == null)
        {
            return "NA";
        }

        CultureInfo cultureInfo = summary.GetCultureInfo();
        RatioStyle ratioStyle = summary.Style?.RatioStyle ?? RatioStyle.Value;

        bool advancedPrecision = IsNonBaselinesPrecise(summary, baselineMetrics, benchmarkCase);
        return ratioStyle switch
        {
            RatioStyle.Value => ratio.Value.ToString(advancedPrecision ? "N3" : "N2", cultureInfo),
            RatioStyle.Percentage => isBaseline
                ? ""
                :
                ratio.Value >= 1.0
                    ?
                    "+" + ((ratio.Value - 1.0) * 100).ToString(advancedPrecision ? "N1" : "N0", cultureInfo) + "%"
                    : "-" + ((1.0 - ratio.Value) * 100).ToString(advancedPrecision ? "N1" : "N0", cultureInfo) + "%",
            RatioStyle.Trend => isBaseline ? "" :
                ratio.Value >= 1.0 ? ratio.Value.ToString(advancedPrecision ? "N3" : "N2", cultureInfo) + "x more" :
                invertedRatio == null ? "NA" :
                invertedRatio.Value.ToString(advancedPrecision ? "N3" : "N2", cultureInfo) + "x less",
            _ => throw new ArgumentOutOfRangeException(nameof(summary), ratioStyle, "RatioStyle is not supported")
        };
    }

    private static bool IsNonBaselinesPrecise(Summary? summary, IReadOnlyDictionary<string, Metric> baselineMetric, BenchmarkCase benchmarkCase)
    {
        if (summary == null)
        {
            return false;
        }

        string? logicalGroupKey = summary.GetLogicalGroupKey(benchmarkCase);
        if (logicalGroupKey == null)
        {
            return false;
        }

        IEnumerable<BenchmarkCase> nonBaselines = summary.GetNonBaselines(logicalGroupKey);
        return nonBaselines.Any(c => GetAllocationRatio(summary[c]?.Metrics, baselineMetric) is > 0 and < 0.01);
    }

    private static double? GetAllocationRatio(
        IReadOnlyDictionary<string, Metric>? current,
        IReadOnlyDictionary<string, Metric>? baseline)
    {
        double? currentBytes = GetAllocatedBytes(current);
        double? baselineBytes = GetAllocatedBytes(baseline);

        if (currentBytes == null || baselineBytes == null)
        {
            return null;
        }

        if (baselineBytes == 0)
        {
            return null;
        }

        return currentBytes / baselineBytes;
    }

    private static double? GetAllocatedBytes(IReadOnlyDictionary<string, Metric>? metrics)
        => metrics
            ?.Where(x => x.Key is "AllocatedNativeMemoryDescriptor" or "Allocated Memory")
            .Sum(x => x.Value.Value);

    public override ColumnCategory Category => ColumnCategory.Metric;
    public override int PriorityInCategory => int.MaxValue - 99;
    public override bool IsNumeric => true;
    public override UnitType UnitType => UnitType.Dimensionless;
    public override string Legend => "Total allocated memory ratio distribution ([Current]/[Baseline])";
}