using System.Collections;
using System.Text.Json.Serialization;

namespace ToDoApp.Model;

public sealed class TasksCollection : IReadOnlyList<ToDoTask>
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
        var rnd = new Random();
        int generated;
        do
        {
            generated = rnd.Next();
        } while (_tasks.Any(task => task.Id == generated));
        return generated;
    }
    
    public IEnumerator<ToDoTask> GetEnumerator() => _tasks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => _tasks.Count;

    public ToDoTask this[int index] => _tasks[index];

    public TasksCollection(params ToDoTask[] initTasks)
        : this((IEnumerable<ToDoTask>)initTasks)
    { }
    
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