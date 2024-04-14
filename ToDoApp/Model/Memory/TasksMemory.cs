namespace ToDoApp.Model;

public class TasksMemory : IMemory<TasksCollection>
{
    private readonly string _dirPath;
    
    private readonly ISerializer<TasksCollection> _serializer;

    private readonly Dictionary<DateOnly, DateTasksMemory> _dateMemories = new();
    
    public void Save(TasksCollection data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        var grouped = data.GroupBy(task => task.Date);

        foreach (var oneDateTasks in grouped)
        {
            var date = oneDateTasks.Key;
            var dateCollection = new TasksCollection(oneDateTasks);
            
            if (!_dateMemories.TryGetValue(date, out var memory))
            {
                var dateFileName = DateTasksMemory.ResolveFileName(date);
                var dateFilePath = Path.Combine(_dirPath, dateFileName);
                
                memory = new DateTasksMemory(dateFilePath, date, _serializer);
                _dateMemories.Add(date, memory);
            }

            memory.Save(dateCollection);
        }
    }

    public async Task SaveAsync(TasksCollection data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        var grouped = data.GroupBy(task => task.Date);

        foreach (var oneDateTasks in grouped)
        {
            var date = oneDateTasks.Key;
            var dateCollection = new TasksCollection(oneDateTasks);
            
            if (!_dateMemories.TryGetValue(date, out var memory))
            {
                var dateFileName = DateTasksMemory.ResolveFileName(date);
                var dateFilePath = Path.Combine(_dirPath, dateFileName);
                
                memory = new DateTasksMemory(dateFilePath, date, _serializer);
                _dateMemories.Add(date, memory);
            }

            await memory.SaveAsync(dateCollection);
        }
    }

    public TasksCollection Load()
    {
        var files = Directory.GetFiles(_dirPath);
        
        var accumulator = Enumerable.Empty<ToDoTask>();

        foreach (var filePath in files)
        {
            var fileName = Path.GetFileName(filePath);
            if (!DateTasksMemory.TryResolveDate(fileName, out var date))
                continue;

            if (!_dateMemories.TryGetValue(date.Value, out var memory))
            {
                memory = new DateTasksMemory(filePath, date.Value, _serializer);
            }

            var dateTasks = memory.Load();
            accumulator = accumulator.Concat(dateTasks);
        }

        return new TasksCollection(accumulator);
    }

    public async Task<TasksCollection> LoadAsync()
    {
        var files = Directory.GetFiles(_dirPath);
        
        var accumulator = Enumerable.Empty<ToDoTask>();

        foreach (var filePath in files)
        {
            var fileName = Path.GetFileName(filePath);
            if (!DateTasksMemory.TryResolveDate(fileName, out var date))
                continue;

            if (!_dateMemories.TryGetValue(date.Value, out var memory))
            {
                memory = new DateTasksMemory(filePath, date.Value, _serializer);
            }

            var dateTasks = await memory.LoadAsync();
            accumulator = accumulator.Concat(dateTasks);
        }

        return new TasksCollection(accumulator);
    }

    public TasksMemory(string dirPath, ISerializer<TasksCollection> serializer)
    {
        ArgumentNullException.ThrowIfNull(dirPath, nameof(dirPath));
        ArgumentNullException.ThrowIfNull(serializer, nameof(serializer));

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        _dirPath = dirPath;
        _serializer = serializer;
    }
}