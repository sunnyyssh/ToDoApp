namespace ToDoApp.Model;

public abstract class FileMemory<TData> : IMemory<TData>
{
    private readonly ISerializer<TData> _serializer;
    
    public string FilePath { get; }

    public virtual void Save(TData data)
    {
        var serialized = _serializer.Serialize(data);
        
        File.WriteAllText(FilePath, serialized);
    }

    public virtual async Task SaveAsync(TData data)
    {
        var serialized = await _serializer.SerializeAsync(data);
        
        await File.WriteAllTextAsync(FilePath, serialized);
    }

    public virtual TData Load()
    {
        var stringData = File.ReadAllText(FilePath);

        return _serializer.Deserialize(stringData);
    }

    public virtual async Task<TData> LoadAsync()
    {
        var stringData = await File.ReadAllTextAsync(FilePath);

        return await _serializer.DeserializeAsync(stringData);
    }

    protected FileMemory(string filePath, ISerializer<TData> serializer)
    {
        ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));
        ArgumentNullException.ThrowIfNull(serializer, nameof(serializer));

        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }
        
        FilePath = filePath;
        _serializer = serializer;
    }
}