using System.Text.Json;
using System.Text.Json.Serialization;
using Sunnyyssh.ConsoleUI;

namespace ToDoApp.View;

[JsonConverter(typeof(ThemeConverter))]
public class Theme
{
    public static readonly Theme Dark = new Theme("DARK", "Dark");
    
    public static readonly Theme Light = new Theme("LIGHT", "Light");

    public string UniqueName { get; }

    public string HumanizedName { get; }

    private Theme(string uniqueName, string humanizedName)
    {
        UniqueName = uniqueName;
        HumanizedName = humanizedName;
    }

    private class ThemeConverter : JsonConverter<Theme>
    {
        public override Theme? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();

            if (str == Dark.UniqueName)
                return Dark;
            
            if (str == Light.UniqueName)
                return Light;
            
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Theme value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.UniqueName);
        }
    } 
}