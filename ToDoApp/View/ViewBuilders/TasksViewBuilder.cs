using System.Collections.Immutable;
using Sunnyyssh.ConsoleUI;
using ToDoApp.ViewModel;

namespace ToDoApp.View;

public sealed class TasksViewBuilder : IViewBuilder
{
    public string Name { get; } = "Previous tasks";
    
    public IUIElementBuilder InitializeBuilder(ViewContextSide context)
    {
        var headers = ImmutableList.Create("№", "Name", "Description", "Date");
        var gridRows = GridRowDefinition.From(
            new[]
            {
                GridRow.FromHeight(1),
                GridRow.FromRowRelation(1.0),
                GridRow.FromRowRelation(1.0),
                GridRow.FromRowRelation(1.0),
                GridRow.FromRowRelation(1.0),
                GridRow.FromRowRelation(1.0),
                GridRow.FromRowRelation(1.0),
                GridRow.FromRowRelation(1.0),
                GridRow.FromRowRelation(1.0),
            });
        var gridColumns = GridColumnDefinition.From(10, 30, 40, 20);
        var gridDefinition = new GridDefinition(gridRows, gridColumns);

        var initData = Enumerable.Range(0, 100)
            .Select(row =>
                {
                    var task = row < context.Tasks.Count ? context.Tasks[row] : null;
                    return task is null
                        ? ImmutableList.Create($"{row + 1}", null, null, null)
                        : ImmutableList.Create(
                            $"{row + 1}",
                            $"{task.Name}",
                            $"{task.Description}",
                            $"{task.Date:MM/dd/yyyy}");
                }
            )
            .ToImmutableList();
        
        var tasksTableBuilder = new ViewTableBuilder(Size.FullSize, headers, gridDefinition, initData);
        
        return tasksTableBuilder;
    }

    public void InitializeView(UIElement element, ViewContextSide context)
    { }
}