namespace SharpIDE.Application.Features.Events;

public static class GlobalEvents
{
	public static event Func<Task> ProjectsRunningChanged =  () => Task.CompletedTask;
	public static void InvokeProjectsRunningChanged() => ProjectsRunningChanged?.Invoke();
}
