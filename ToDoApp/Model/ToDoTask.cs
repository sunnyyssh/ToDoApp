using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

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
    [JsonConverter(typeof(DateOnlyConverter))]
    public DateOnly Date { get; }

    [JsonConstructor]
    public ToDoTask(int id, string name, string? description, DateOnly date)
    {
        Id = id;
        Name = name;
        Description = description;
        Date = date;
    }
    
    private class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private static readonly Regex DateParseRegex = new Regex(@"(?<day>\d+)-(?<month>\d+)-(?<year>\d+)"); 
        
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            if (str is null)
                return DateOnly.MinValue;
            
            var match = DateParseRegex.Match(str);
        
            if (!match.Success)
                return DateOnly.MinValue;

            if (!int.TryParse(match.Groups["day"].Value, out int day))
                return DateOnly.MinValue;
            
            if (!int.TryParse(match.Groups["month"].Value, out int month))
                return DateOnly.MinValue;
            
            if (!int.TryParse(match.Groups["year"].Value, out int year))
                return DateOnly.MinValue;

            return new DateOnly(year, month, day);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"{value.Day}-{value.Month}-{value.Year}");
        }
    }
}