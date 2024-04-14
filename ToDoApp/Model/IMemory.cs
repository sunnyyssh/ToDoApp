namespace ToDoApp.Model;

public interface IMemory<TData>
{
    public void Save(TData data);

    public Task SaveAsync(TData data);

    public TData Load();

    public Task<TData> LoadAsync();
}