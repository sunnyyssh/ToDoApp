namespace ToDoApp.Model;

public abstract class FileMemory<TData> : IMemory<TData>
{
    private readonly ISerializer<TData> _serializer;
    private readonly TData _defaultValue;

    public string FilePath { get; }

    public virtual void Save(TData data)
    {
        var serialized = _serializer.Serialize(data);
        
        File.AppendAllText(FilePath, serialized);
    }

    public virtual async Task SaveAsync(TData data)
    {
        var serialized = await _serializer.SerializeAsync(data);
        
        await File.AppendAllTextAsync(FilePath, serialized);
    }

    public void Rewrite(TData data)
    {
        var serialized = _serializer.Serialize(data);
        
        File.WriteAllText(FilePath, serialized);
    }

    public async Task RewriteAsync(TData data)
    {
        var serialized = await _serializer.SerializeAsync(data);
        
        await File.WriteAllTextAsync(FilePath, serialized);
    }

    public virtual TData Load()
    {
        var stringData = File.ReadAllText(FilePath);

        return _serializer.Deserialize(stringData) ?? _defaultValue;
    }

    public virtual async Task<TData> LoadAsync()
    {
        var stringData = await File.ReadAllTextAsync(FilePath);

        return await _serializer.DeserializeAsync(stringData) ?? _defaultValue;
    }

    protected FileMemory(string filePath, ISerializer<TData> serializer, TData defaultValue)
    {
        ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));
        ArgumentNullException.ThrowIfNull(serializer, nameof(serializer));

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, serializer.Serialize(defaultValue));
        }
        
        FilePath = filePath;
        _serializer = serializer;
        _defaultValue = defaultValue;
    }
}