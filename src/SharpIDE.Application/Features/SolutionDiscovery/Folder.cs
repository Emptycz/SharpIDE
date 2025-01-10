namespace SharpIDE.Application.Features.SolutionDiscovery;

public class Folder
{
	public required string Name { get; set; }
	public List<Folder> Folders { get; set; } = [];
	public List<MyFile> Files { get; set; } = [];
}

public class MyFile
{
	public required string Name { get; set; }
}
