using ToDoApp.Model;

namespace ToDoApp.ViewModel;

public sealed class ModelContextSide
{
    public Settings Settings { get; private set; }

    public TasksCollection Tasks { get; private set; }

    internal void BindViewSide(ViewContextSide viewSide, TasksCollectionChanger collectionChanger)
    {
        viewSide.SettingsUpdated += update => SettingsUpdated?.Invoke(update);
        
        collectionChanger.TaskAdded += newTask => TaskAdded?.Invoke(newTask);
        collectionChanger.TaskRemoved += toRemove => TaskRemoved?.Invoke(toRemove);
    }

    public event SettingsUpdatedEventHandler? SettingsUpdated;

    public event TaskAddedEventHandler? TaskAdded;

    public event TaskRemovedEventHandler? TaskRemoved;

    internal ModelContextSide(Settings settings, TasksCollection tasks)
    {
        Settings = settings;
        Tasks = tasks;
    }
}