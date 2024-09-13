using System.Collections.Concurrent;

namespace Roaring.Net.Benchmarks;

internal static class TestFileLoader
{
    public static async Task<List<uint[]>> GetValuesForPath(string path)
    {
        var filePaths = Directory.GetFiles(path);

        ConcurrentDictionary<string, uint[]> fileValues = new();

        await Parallel.ForEachAsync(filePaths, async (filePath, cancellationToken) =>
        {
            var content = await File.ReadAllTextAsync(filePath, cancellationToken);
            var values = content
                .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(uint.Parse)
                .ToArray();

            fileValues[filePath] = values;
        });

        return fileValues.Values.ToList();
    }
}