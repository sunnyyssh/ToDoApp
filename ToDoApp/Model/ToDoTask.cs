using System.Text.Json.Serialization;

namespace ToDoApp.Model;

public sealed class ToDoTask
{
    [JsonInclude]
    [JsonPropertyName("id")]
    public int Id { get; }
    
    [JsonInclude]
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonInclude]
    [JsonPropertyName("description")]
    public string? Description { get; }
    
    [JsonInclude]
    [JsonPropertyName("date")]
    public DateOnly Date { get; }

    [JsonConstructor]
    public ToDoTask(int id, string name, string? description, DateOnly date)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}