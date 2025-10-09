using SharpIDE.Application.Features.Events;
using SharpIDE.Application.Features.SolutionDiscovery.VsPersistence;

namespace SharpIDE.Application.Features.FileWatching;

public class IdeFileChangeHandler
{
	public SharpIdeSolutionModel SolutionModel { get; set; } = null!;
	public IdeFileChangeHandler()
	{
		GlobalEvents.Instance.FileSystemWatcherInternal.FileChanged.Subscribe(OnFileChanged);
	}

	private async Task OnFileChanged(string filePath)
	{
		var sharpIdeFile = SolutionModel.AllFiles.SingleOrDefault(f => f.Path == filePath);
		if (sharpIdeFile is null) return;
		if (sharpIdeFile.SuppressDiskChangeEvents.Value is true) return;
		Console.WriteLine($"IdeFileChangeHandler: Changed - {filePath}");
		await sharpIdeFile.FileContentsChangedExternallyFromDisk.InvokeParallelAsync();
	}
}
