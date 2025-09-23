using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using SharpIDE.Application.Features.SolutionDiscovery.VsPersistence;

namespace SharpIDE.Application.Features.Search;

public static class SearchService
{
	public static async Task<List<SearchResult>> FindInFiles(SharpIdeSolutionModel solutionModel, string searchTerm)
	{
		if (searchTerm.Length < 4)
		{
			return [];
		}

		var timer = Stopwatch.StartNew();
		var files = solutionModel.AllFiles;
		ConcurrentBag<SearchResult> results = [];
		await Parallel.ForEachAsync(files, async (file, ct) =>
			{
				await foreach (var (index, line) in File.ReadLinesAsync(file.Path, ct).Index().WithCancellation(ct))
				{
					if (line.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
					{
						results.Add(new SearchResult
						{
							File = file,
							LineNumber = index + 1,
							LineText = line.Trim()
						});
					}
				}
			}
		);
		timer.Stop();
		Console.WriteLine($"Search completed in {timer.ElapsedMilliseconds} ms. Found {results.Count} results.");
		return results.ToList();
	}
}
