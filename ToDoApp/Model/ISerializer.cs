namespace ToDoApp.Model;

public interface ISerializer<TData>
{
    public string Serialize(TData data);

    public Task<string> SerializeAsync(TData data);

    public TData? Deserialize(string from);
    
    public Task<TData?> DeserializeAsync(string from);
}