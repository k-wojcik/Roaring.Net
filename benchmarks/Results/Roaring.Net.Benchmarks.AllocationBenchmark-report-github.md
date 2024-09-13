```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4169/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900K, 1 CPU, 32 logical and 24 physical cores
.NET SDK 8.0.401
  [Host]            : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  ShortRun-.NET 6.0 : .NET 6.0.33 (6.0.3324.36610), X64 RyuJIT AVX2
  ShortRun-.NET 8.0 : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2

IterationCount=3  LaunchCount=1  WarmupCount=3  

```
| Method             | Job               | Runtime  | Mean        | Error        | StdDev    | Ratio  | RatioSD | Gen0     | Allocated native memory | Native memory leak | Gen1     | Gen2     | Allocated    | Alloc Ratio | Total Allocated | Total Allocated Ratio |
|------------------- |------------------ |--------- |------------:|-------------:|----------:|-------:|--------:|---------:|------------------------:|-------------------:|---------:|---------:|-------------:|------------:|----------------:|----------------------:|
| Roaring32          | ShortRun-.NET 6.0 | .NET 6.0 | 15,653.7 μs |  1,234.09 μs |  67.64 μs |  72.77 |    0.63 |        - |               11,718 KB |                  - |        - |        - |      7.97 KB |        1.00 |     11725.66 KB |                  5.26 |
| OptimizedRoaring32 | ShortRun-.NET 6.0 | .NET 6.0 |    215.1 μs |     35.34 μs |   1.94 μs |   1.00 |    0.01 |   0.2441 |                2,221 KB |                  - |        - |        - |      7.94 KB |        1.00 |      2229.36 KB |                  1.00 |
| FrozenRoaring32    | ShortRun-.NET 6.0 | .NET 6.0 |    213.5 μs |     17.29 μs |   0.95 μs |   0.99 |    0.01 |  33.2031 |                2,224 KB |                  - |  31.2500 |  31.2500 |     39.34 KB |        4.96 |      2263.81 KB |                  1.02 |
| HashSet            | ShortRun-.NET 6.0 | .NET 6.0 | 53,164.8 μs | 15,845.84 μs | 868.56 μs | 247.16 |    3.99 | 500.0000 |                       - |                  - | 400.0000 | 300.0000 |  114561.6 KB |   14,432.96 |     114561.6 KB |                 51.39 |
| FrozenSet          | ShortRun-.NET 6.0 | .NET 6.0 |          NA |           NA |        NA |      ? |       ? |       NA |                      NA |                 NA |       NA |       NA |           NA |           ? |            0 KB |                     ? |
| Array              | ShortRun-.NET 6.0 | .NET 6.0 |  8,214.2 μs |  3,678.95 μs | 201.66 μs |  38.19 |    0.86 | 156.2500 |                       - |                  - | 125.0000 |  62.5000 |  27046.19 KB |    3,407.39 |     27046.19 KB |                 12.13 |
|                    |                   |          |             |              |           |        |         |          |                         |                    |          |          |              |             |                 |                       |
| Roaring32          | ShortRun-.NET 8.0 | .NET 8.0 | 15,686.6 μs |    872.69 μs |  47.84 μs |  71.52 |    0.68 |        - |               11,718 KB |                  - |        - |        - |      7.95 KB |        1.00 |     11725.64 KB |                  5.26 |
| OptimizedRoaring32 | ShortRun-.NET 8.0 | .NET 8.0 |    219.3 μs |     42.55 μs |   2.33 μs |   1.00 |    0.01 |   0.2441 |                2,221 KB |                  - |        - |        - |      7.94 KB |        1.00 |      2229.36 KB |                  1.00 |
| FrozenRoaring32    | ShortRun-.NET 8.0 | .NET 8.0 |    201.5 μs |     41.28 μs |   2.26 μs |   0.92 |    0.01 |  33.6914 |                2,224 KB |                  - |  31.7383 |  31.7383 |     39.33 KB |        4.96 |       2263.8 KB |                  1.02 |
| HashSet            | ShortRun-.NET 8.0 | .NET 8.0 | 30,044.4 μs |  1,623.19 μs |  88.97 μs | 136.98 |    1.30 | 357.1429 |                    0 KB |               0 KB | 285.7143 | 142.8571 | 114561.55 KB |   14,432.95 |    114561.58 KB |                 51.39 |
| FrozenSet          | ShortRun-.NET 8.0 | .NET 8.0 | 71,869.5 μs |  4,184.11 μs | 229.35 μs | 327.67 |    3.13 | 333.3333 |                    0 KB |                  - |        - |        - | 203818.65 KB |   25,677.94 |     203818.7 KB |                 91.42 |
| Array              | ShortRun-.NET 8.0 | .NET 8.0 |  3,344.6 μs |  2,401.86 μs | 131.65 μs |  15.25 |    0.54 | 175.7813 |                    0 KB |                  - | 152.3438 |  78.1250 |  27046.17 KB |    3,407.39 |     27046.18 KB |                 12.13 |

Benchmarks with issues:
  AllocationBenchmark.FrozenSet: ShortRun-.NET 6.0(Runtime=.NET 6.0, IterationCount=3, LaunchCount=1, WarmupCount=3)
