using System.Collections.Generic;
using System.Linq;
using GDExtensionBindgen;
using Godot;
using SharpIDE.Application.Features.Events;
using SharpIDE.Application.Features.SolutionDiscovery.VsPersistence;

namespace SharpIDE.Godot.Features.Run;

public partial class RunPanel : Control
{
	private TabBar _tabBar = null!;
	private Panel _tabsPanel = null!;
	
	[Export]
	public Texture2D RunningIcon { get; set; } = null!;
	
	private PackedScene _runPanelTabScene = GD.Load<PackedScene>("res://Features/Run/RunPanelTab.tscn");
	public override void _Ready()
	{
		_tabBar = GetNode<TabBar>("%TabBar");
		_tabBar.ClearTabs();
		_tabsPanel = GetNode<Panel>("%TabsPanel");
		GlobalEvents.ProjectStartedRunning += async projectModel =>
		{
			await this.InvokeAsync(() => ProjectStartedRunning(projectModel));
		};
		GlobalEvents.ProjectStoppedRunning += async projectModel =>
		{
			await this.InvokeAsync(() => ProjectStoppedRunning(projectModel));
		};
	}

	public void ProjectStartedRunning(SharpIdeProjectModel projectModel)
	{
		var existingRunPanelTab = _tabsPanel.GetChildren().OfType<RunPanelTab>().SingleOrDefault(s => s.Project == projectModel);
		if (existingRunPanelTab != null)
		{
			_tabBar.SetTabIcon(existingRunPanelTab.TabBarTab, RunningIcon);
			existingRunPanelTab.ClearTerminal();
			existingRunPanelTab.StartWritingFromProjectOutput();
			return;
		}
		
		var runPanelTab = _runPanelTabScene.Instantiate<RunPanelTab>();
		runPanelTab.Project = projectModel;
		_tabBar.AddTab(projectModel.Name);
		var tabIdx = _tabBar.GetTabCount() - 1;
		runPanelTab.TabBarTab = tabIdx;
		_tabBar.SetTabIcon(runPanelTab.TabBarTab, RunningIcon);
		_tabsPanel.AddChild(runPanelTab);
		runPanelTab.StartWritingFromProjectOutput();
	}
	
	public void ProjectStoppedRunning(SharpIdeProjectModel projectModel)
	{
		var runPanelTab = _tabsPanel.GetChildren().OfType<RunPanelTab>().Single(s => s.Project == projectModel);
		_tabBar.SetTabIcon(runPanelTab.TabBarTab, null);
	}
}