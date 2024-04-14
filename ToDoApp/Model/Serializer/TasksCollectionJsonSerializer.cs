using System.Text.Json;

namespace ToDoApp.Model;

public class TasksCollectionJsonSerializer : JsonSerializer<TasksCollection>
{
    public override TasksCollection? Deserialize(string from)
    {
        var tasks = JsonSerializer.Deserialize<ToDoTask[]>(from);
        return tasks is not null ? new TasksCollection(tasks) : null;
    }

    public override async Task<TasksCollection?> DeserializeAsync(string from)
    {
        await using var memoryStream = new MemoryStream();
        
        var writer = new StreamWriter(memoryStream);
        await writer.WriteAsync(from);
        await writer.FlushAsync();

        memoryStream.Position = 0;

        var tasks = await JsonSerializer.DeserializeAsync<ToDoTask[]>(memoryStream);

        return tasks is not null ? new TasksCollection(tasks) : null;
    }
}