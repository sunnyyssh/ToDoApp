using Sunnyyssh.ConsoleUI;
using ToDoApp.ViewModel;

namespace ToDoApp.View;

public interface IViewBuilder
{
    public string Name { get; }
    
    public IUIElementBuilder InitializeBuilder(ViewContextSide context);

    public void InitializeView(UIElement element, ViewContextSide context);
}