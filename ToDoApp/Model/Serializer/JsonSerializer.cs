using System.Text.Json;

namespace ToDoApp.Model;

public class JsonSerializer<TData> : ISerializer<TData>
{
    public virtual string Serialize(TData data)
    {
        return JsonSerializer.Serialize(data);
    }

    public virtual async Task<string> SerializeAsync(TData data)
    {
        await using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, data);

        memoryStream.Position = 0;
        
        var reader = new StreamReader(memoryStream);
        return await reader.ReadToEndAsync();
    }

    public virtual TData? Deserialize(string from)
    {
        return JsonSerializer.Deserialize<TData>(from);
    }

    public virtual async Task<TData?> DeserializeAsync(string from)
    {
        await using var memoryStream = new MemoryStream();
        
        var writer = new StreamWriter(memoryStream);
        await writer.WriteAsync(from);
        await writer.FlushAsync();

        memoryStream.Position = 0;
        
        return await JsonSerializer.DeserializeAsync<TData>(memoryStream);
    }
}