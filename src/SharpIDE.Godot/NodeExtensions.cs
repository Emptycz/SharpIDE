using Godot;

namespace SharpIDE.Godot;

public static class NodeExtensions
{
    extension(Node node)
    {
        public Task InvokeAsync(Action workItem)
        {
            var taskCompletionSource = new TaskCompletionSource();
            //WorkerThreadPool.AddTask();
            Callable.From(() =>
            {
                try
                {
                    workItem();
                    taskCompletionSource.SetResult();
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                }
            }).CallDeferred();
            return taskCompletionSource.Task;
        }
        
        public Task InvokeAsync(Func<Task> workItem)
        {
            var taskCompletionSource = new TaskCompletionSource();
            //WorkerThreadPool.AddTask();
            Callable.From(async void () =>
            {
                try
                {
                    await workItem();
                    taskCompletionSource.SetResult();
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                }
            }).CallDeferred();
            return taskCompletionSource.Task;
        }
    }
}

public static class GodotTask
{
    extension(Task task)
    {
        public static async Task GodotRun(Action action)
        {
            await Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"Error: {ex}");
                }
            });
        }
    
        public static async Task GodotRun(Func<Task> action)
        {
            await Task.Run(async () =>
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"Error: {ex}");
                }
            });
        }
    }
}