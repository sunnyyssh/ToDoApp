using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDoApp.Model;

public sealed class TasksCollection : IEnumerable<ToDoTask>
{
    public static readonly TasksCollection Empty = new TasksCollection(Enumerable.Empty<ToDoTask>());
    
    private readonly List<ToDoTask> _tasks;

    public ToDoTask[] GetToday()
    {
        return _tasks
            .Where(t => 
                t.Date == DateOnly.FromDateTime(DateTime.Today))
            .ToArray();
    }

    public int GenerateUniqueTaskId()
    {
        throw new NotImplementedException(); // NotImplementedException
    }
    
    public IEnumerator<ToDoTask> GetEnumerator() => _tasks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    [JsonConstructor]
    public TasksCollection(IEnumerable<ToDoTask> initTasks)
    {
        _tasks = initTasks.ToList();
    }
    
    internal TasksCollection(IEnumerable<ToDoTask> initTasks, TasksCollectionChanger changer)
    {
        _tasks = initTasks.ToList();

        changer.TaskAdded += t => _tasks.Add(t);
        
        changer.TaskRemoved += t => _tasks.Remove(t);
    }
}