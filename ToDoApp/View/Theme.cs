using System.Text.Json;
using System.Text.Json.Serialization;
using Sunnyyssh.ConsoleUI;

namespace ToDoApp.View;

[JsonConverter(typeof(ThemeConverter))]
public class Theme
{
    public static readonly Theme Dark = new Theme("DARK", "Dark")
    {
        DefaultBackground = Color.Black,
        DefaultForeground = Color.White,
        TableCellFocusedBackground = Color.DarkBlue,
        MenuNotFocusedBackground = Color.DarkGray,
        MenuFocusedBackground = Color.Gray,
        MenuChosenBackground = Color.DarkBlue,
        MenuChosenFocusedBackground = Color.Blue, 
        MessageForeground = Color.Blue,
        AdditionalBackground = Color.DarkGray,
        AdditionalForeground = Color.Gray,
        BorderColor = Color.Gray,
        MenuBorderColor = Color.White,
        FocusedBorderColor = Color.DarkGray,
    };

    public static readonly Theme Light = new Theme("LIGHT", "Light")
    {
        DefaultBackground = Color.White,
        DefaultForeground = Color.Black,
        TableCellFocusedBackground = Color.Blue,
        MenuNotFocusedBackground = Color.White,
        MenuFocusedBackground = Color.Gray,
        MenuChosenBackground = Color.Blue,
        MenuChosenFocusedBackground = Color.DarkBlue,
        MessageForeground = Color.DarkBlue,
        AdditionalBackground = Color.Gray,
        AdditionalForeground = Color.DarkGray,
        BorderColor = Color.DarkGray,
        MenuBorderColor = Color.Black,
        FocusedBorderColor = Color.Gray,
    };


    public string UniqueName { get; }

    public string HumanizedName { get; }
    
    public Color MenuChosenFocusedBackground { get; init; }

    public Color DefaultBackground { get; init; } 
    
    public Color DefaultForeground { get; init; }
    
    public Color TableCellFocusedBackground { get; init; }
    
    public Color MenuNotFocusedBackground { get; init; }
    
    public Color MenuFocusedBackground { get; init; }
    
    public Color MenuChosenBackground { get; init; }
    
    public Color MenuBorderColor { get; init; }
    
    public Color MessageForeground { get; init; }
    
    public Color AdditionalBackground { get; init; }
    
    public Color AdditionalForeground { get; init; }
    
    public Color BorderColor { get; init; }
    
    public Color FocusedBorderColor { get; init; }

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