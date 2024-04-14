using System.Text.Json.Serialization;
using ToDoApp.View;

namespace ToDoApp.ViewModel;

public sealed class Settings
{
    [JsonInclude]
    [JsonPropertyName("theme")]
    public Theme Theme { get; }

    [JsonConstructor]
    public Settings(Theme theme)
    {
        Theme = theme;
    }
}