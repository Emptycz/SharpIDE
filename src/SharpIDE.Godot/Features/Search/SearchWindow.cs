using Godot;
using SharpIDE.Application.Features.Search;
using SharpIDE.Application.Features.SolutionDiscovery.VsPersistence;

namespace SharpIDE.Godot.Features.Search;

public partial class SearchWindow : PopupPanel
{
    private LineEdit _lineEdit = null!;
    private VBoxContainer _searchResultsContainer = null!;
    public SharpIdeSolutionModel Solution { get; set; } = null!;
	private readonly PackedScene _searchResultEntryScene = ResourceLoader.Load<PackedScene>("res://Features/Search/SearchResultComponent.tscn");
    
    public override void _Ready()
    {
        _lineEdit = GetNode<LineEdit>("%SearchLineEdit");
        _searchResultsContainer = GetNode<VBoxContainer>("%SearchResultsVBoxContainer");
        _lineEdit.TextChanged += OnTextChanged;
    }

    private async void OnTextChanged(string newText)
    {
        GD.Print("Search text changed");
        await Task.GodotRun(() => Search(newText));
    }

    private async Task Search(string text)
    {
        var result = await SearchService.FindInFiles(Solution, text);
        await this.InvokeAsync(() =>
        {
            _searchResultsContainer.GetChildren().ToList().ForEach(s => s.QueueFree());
            foreach (var searchResult in result)
            {
                var result = _searchResultEntryScene.Instantiate<SearchResultComponent>();
                result.Result = searchResult;
                _searchResultsContainer.AddChild(result);
            }
        });
    }
}