using ToDoApp.View;

namespace ToDoApp.ViewModel;

public sealed class Settings
{
    public Theme Theme { get; }

    public Settings(Theme theme)
    {
        Theme = theme;
    }
}