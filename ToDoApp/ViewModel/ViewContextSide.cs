using ToDoApp.Model;

namespace ToDoApp.ViewModel;

public delegate void SettingsUpdatedEventHandler(Settings update);

public sealed class ViewContextSide
{
    private readonly TasksCollectionChanger _tasksChanger;
    
    public Settings Settings { get; private set; }
    
    public TasksCollection Tasks { get; }

    public void UpdateSettings(Settings update)
    {
        ArgumentNullException.ThrowIfNull(update, nameof(update));

        Settings = update;
        SettingsUpdated?.Invoke(update);
    }

    public void AddTask(ToDoTask newTask) => _tasksChanger.Add(newTask);

    public void RemoveTask(ToDoTask toRemove) => _tasksChanger.Remove(toRemove);

    public event SettingsUpdatedEventHandler? SettingsUpdated;

    internal ViewContextSide(Settings initSettings, TasksCollection initTasks, TasksCollectionChanger tasksChanger)
    {
        _tasksChanger = tasksChanger;
        Settings = initSettings;
        Tasks = initTasks;
    }
}