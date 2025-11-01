using Godot;
using SharpIDE.Application.Features.Nuget;
using SharpIDE.Application.Features.SolutionDiscovery.VsPersistence;

namespace SharpIDE.Godot.Features.Nuget;

public partial class NugetPanel : Control
{
    private VBoxContainer _installedPackagesVboxContainer = null!;
    private VBoxContainer _implicitlyInstalledPackagesItemList = null!;
    private VBoxContainer _availablePackagesItemList = null!;
    
    public SharpIdeSolutionModel? Solution { get; set; }
    
    [Inject] private readonly NugetClientService _nugetClientService = null!;

    public override void _Ready()
    {
        _installedPackagesVboxContainer = GetNode<VBoxContainer>("%InstalledPackagesVBoxContainer");
        _implicitlyInstalledPackagesItemList = GetNode<VBoxContainer>("%ImplicitlyInstalledPackagesVBoxContainer");
        _availablePackagesItemList = GetNode<VBoxContainer>("%AvailablePackagesVBoxContainer");

        _ = Task.GodotRun(async () =>
        {
            await Task.Delay(300);
            var result = await _nugetClientService.GetTop100Results(Solution!.DirectoryPath);
            ;
        });
    }
}