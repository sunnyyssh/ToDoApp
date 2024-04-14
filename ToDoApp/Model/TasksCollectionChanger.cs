namespace ToDoApp.Model;

public delegate void TaskAddedEventHandler(ToDoTask added);

public delegate void TaskRemovedEventHandler(ToDoTask deleted);

internal class TasksCollectionChanger
{
    public void Add(ToDoTask newTask)
    {
        ArgumentNullException.ThrowIfNull(newTask, nameof(newTask));

        TaskAdded?.Invoke(newTask);
    }

    public void Remove(ToDoTask toRemove)
    {
        ArgumentNullException.ThrowIfNull(toRemove, nameof(toRemove));

        TaskRemoved?.Invoke(toRemove);
    }

    public event TaskAddedEventHandler? TaskAdded;
    
    public event TaskRemovedEventHandler? TaskRemoved;
}