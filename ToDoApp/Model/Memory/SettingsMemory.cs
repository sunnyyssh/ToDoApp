using ToDoApp.ViewModel;

namespace ToDoApp.Model;

public class SettingsMemory : FileMemory<Settings>
{
    public SettingsMemory(string filePath, ISerializer<Settings> serializer, Settings defaultSettings) 
        : base(filePath, serializer, defaultSettings)
    { }
}