using Sunnyyssh.ConsoleUI;

namespace ToDoApp.View;

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
}