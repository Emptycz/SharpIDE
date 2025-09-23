using SharpIDE.Application.Features.SolutionDiscovery;

namespace SharpIDE.Application.Features.Search;

public class SearchResult
{
	public required SharpIdeFile File { get; set; }
	public required int LineNumber { get; set; }
	public required string LineText { get; set; }
}
