using ToDoApp.ViewModel;

namespace ToDoApp.Model;

public class SettingsMemory : FileMemory<Settings>
{
    public SettingsMemory(string filePath, ISerializer<Settings> serializer) 
        : base(filePath, serializer)
    { }
}