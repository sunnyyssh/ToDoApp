using ToDoApp.Model;
using ToDoApp.View;
using ToDoApp.ViewModel;

namespace ToDoApp;

internal static class Program
{
    private static readonly string TasksDirectory = "tasks";
    
    private static readonly string SettingsFilePath = "settings.settings";
    private static void Main(string[] args)
    {
        var tasksSerializer = new TasksCollectionJsonSerializer();
        var tasksMemory = new TasksMemory(TasksDirectory, tasksSerializer);

        var settingsSerializer = new JsonSerializer<Settings>();
        var settingsMemory = new SettingsMemory(SettingsFilePath, settingsSerializer, new Settings(Theme.Dark));
        
        var settings = settingsMemory.Load();
        var initTasks = tasksMemory.Load();
        var context = new Context(settings, initTasks);
        
        var builder = new ToDoAppBuilder(context.ViewSide);
        builder.AddView(new TodayViewBuilder())
            .AddView(new TasksViewBuilder())
            .AddView(new SettingsViewBuilder());

        context.ModelSide.SettingsUpdated += async update => await settingsMemory.RewriteAsync(update);
        context.ModelSide.TaskAdded += async _ => await tasksMemory.RewriteAsync(context.ModelSide.Tasks);
        
        var app = builder.Build();
        
        app.Run();

        Thread.Sleep(15000);
        app.Wait();
    }
}