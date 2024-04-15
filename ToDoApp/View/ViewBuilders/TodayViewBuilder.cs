using System.Collections.Immutable;
using Sunnyyssh.ConsoleUI;
using ToDoApp.Model;
using ToDoApp.ViewModel;

namespace ToDoApp.View;

public sealed class TodayViewBuilder : IViewBuilder
{
    private const int MaxTodayTasks = 100;
    
    public string Name { get; } = "Today";

    private BuiltUIElement<TextBlock>? _builtMessageBlock;

    private BuiltUIElement<ViewTable>? _builtTasksTable;
    
    private BuiltUIElement<Button>? _builtNewTaskButton;
    
    private BuiltUIElement<TextBox>? _builtNewTaskNameBox;
    
    private BuiltUIElement<TextBox>? _builtNewTaskDescriptionBox;

    public IUIElementBuilder InitializeBuilder(ViewContextSide context)
    {
        var theme = context.Settings.Theme;
        
        var rootGridBuilder = InitializeRootGridBuilder(theme);

        var aggregateGridBuilder = new GridBuilder(Size.FullSize,
            new GridDefinition(GridRowDefinition.From(new[] { GridRow.FromHeight(1), GridRow.FromRowRelation(1.0), }),
                GridColumnDefinition.From(1.0)))
        {
            FocusChangeKeys = ImmutableList<ConsoleKey>.Empty,
            FocusUpKeys = ImmutableList<ConsoleKey>.Empty,
            FocusDownKeys = ImmutableList<ConsoleKey>.Empty,
            FocusRightKeys = ImmutableList<ConsoleKey>.Empty,
            FocusLeftKeys = ImmutableList<ConsoleKey>.Empty,
            FocusFlowLoop = false,
            OverridesFocusFlow = false,
            BorderKind = BorderKind.None,
        };

        var messageBlockBuilder = InitializeMessageBlockBuilder(theme);
        aggregateGridBuilder.Add(messageBlockBuilder, 0, 0);

        var newTaskBoxBuilder = InitializeNewTaskBoxBuilder(theme);
        aggregateGridBuilder.Add(newTaskBoxBuilder, 1, 0);

        rootGridBuilder.Add(aggregateGridBuilder, 0, 0);

        var tasksTableBuilder = InitializeTasksTableBuilder(theme);
        rootGridBuilder.Add(tasksTableBuilder, 1, 0, out _builtTasksTable);

        return rootGridBuilder;
    }

    private ViewTableBuilder InitializeTasksTableBuilder(Theme theme)
    {
        var headers = ImmutableList.Create("№", "Name", "Description");
        var rowDefinition = GridRowDefinition.From(
            new[]
            {
                GridRow.FromHeight(1),
                GridRow.FromRowRelation(1.0),
                GridRow.FromRowRelation(1.0),
                GridRow.FromRowRelation(1.0),
                GridRow.FromRowRelation(1.0), 
                GridRow.FromRowRelation(1.0),
                GridRow.FromRowRelation(1.0), // 6 task rows.
            });
        var columnDefinition = GridColumnDefinition.From(10, 30, 50);
        var gridDefinition = new GridDefinition(rowDefinition, columnDefinition);

        var tableBuilder = new ViewTableBuilder(Size.FullSize, headers, gridDefinition, MaxTodayTasks)
        {
            BorderLineKind = LineKind.Single,
            BorderColorNotFocused = theme.BorderColor,
            HeaderBackground = theme.AdditionalBackground,
            HeaderForeground = theme.AdditionalForeground,
            CellFocusedBackground = theme.TableCellFocusedBackground,
            CellsWordWrap = false,
            MoveUpKeys = ImmutableList.Create(ConsoleKey.UpArrow),
            MoveDownKeys = ImmutableList.Create(ConsoleKey.DownArrow),
            MoveLeftKeys = ImmutableList<ConsoleKey>.Empty,
            MoveRightKeys = ImmutableList<ConsoleKey>.Empty,
            UserEditable = false,
            BorderColorFocused = theme.FocusedBorderColor,
        };
        return tableBuilder;
    }

    private IUIElementBuilder InitializeNewTaskBoxBuilder(Theme theme)
    {
        var gridRows = GridRowDefinition.From(
            new[] { GridRow.FromHeight(1), GridRow.FromRowRelation(1.0), });
        var gridColumns = GridColumnDefinition.From(
            new[] { GridColumn.FromColumnRelation(30), GridColumn.FromColumnRelation(50), GridColumn.FromColumnRelation(20), });
        var gridDefinition = new GridDefinition(gridRows, gridColumns);

        var rootGridBuilder = new GridBuilder(Size.FullSize, gridDefinition)
        {
            BorderKind = BorderKind.None,
            FocusChangeKeys = ImmutableList<ConsoleKey>.Empty,
            FocusDownKeys = ImmutableList<ConsoleKey>.Empty,
            FocusUpKeys = ImmutableList<ConsoleKey>.Empty,
            FocusLeftKeys = ImmutableList<ConsoleKey>.Empty,
            FocusRightKeys = ImmutableList<ConsoleKey>.Empty,
            FocusFlowLoop = false,
            OverridesFocusFlow = false,
        };

        rootGridBuilder
            .Add(new TextBlockBuilder(Size.FullSize)
            {
                Foreground = theme.AdditionalForeground,
                StartingText = "Name",
                TextHorizontalAligning = HorizontalAligning.Left,
                TextVerticalAligning = VerticalAligning.Bottom,
            }, 0, 0)
            .Add(new TextBlockBuilder(Size.FullSize)
            {
                Foreground = theme.AdditionalForeground,
                StartingText = "Description",
                TextHorizontalAligning = HorizontalAligning.Left,
                TextVerticalAligning = VerticalAligning.Bottom,
            }, 0, 1);

        var editableBoxBuilder = new TextBoxBuilder(Size.FullSize)
        {
            BorderKind = BorderKind.SingleLine,
            NotFocusedBorderColor = theme.BorderColor,
            UserEditable = true,
            ShowPressedChars = true,
            WordWrap = true,
            FocusedBorderColor = theme.FocusedBorderColor,
        };
        rootGridBuilder.Add(editableBoxBuilder, 1, 0, out _builtNewTaskNameBox);
        rootGridBuilder.Add(editableBoxBuilder, 1, 1, out _builtNewTaskDescriptionBox);

        var addButtonBuilder = new ButtonBuilder(Size.FullSize)
        {
            BorderKind = BorderKind.SingleLine,
            Text = "Add",
            TextHorizontalAligning = HorizontalAligning.Center,
            TextVerticalAligning = VerticalAligning.Center,
            FocusedBorderColor = theme.FocusedBorderColor,
            ShowPress = true,
            PressedBackground = theme.AdditionalBackground,
            LoseFocusAfterPress = true,
        };
        rootGridBuilder.Add(addButtonBuilder, 1, 2, out _builtNewTaskButton);

        return rootGridBuilder;
    }

    private IUIElementBuilder InitializeMessageBlockBuilder(Theme theme)
    {
        var gridBuilder = new GridBuilder(Size.FullSize,
            new GridDefinition(GridRowDefinition.From(1), GridColumnDefinition.From(2, 1)))
        {
            FocusChangeKeys = ImmutableList<ConsoleKey>.Empty,
            FocusUpKeys = ImmutableList<ConsoleKey>.Empty,
            FocusDownKeys = ImmutableList<ConsoleKey>.Empty,
            FocusRightKeys = ImmutableList<ConsoleKey>.Empty,
            FocusLeftKeys = ImmutableList<ConsoleKey>.Empty,
            FocusFlowLoop = false,
            BorderKind = BorderKind.None,
            OverridesFocusFlow = false,
            OverlappingPriority = OverlappingPriority.Lowest,
        };
        
        var messageBlockBuilder = new TextBlockBuilder(Size.FullSize)
        {
            Background = Color.Transparent,
            Foreground = theme.MessageForeground,
            StartingText = null,
            TextHorizontalAligning = HorizontalAligning.Center,
            TextVerticalAligning = VerticalAligning.Bottom,
            WordWrap = true,
        };
        gridBuilder.Add(messageBlockBuilder, 0, 0, out _builtMessageBlock);

        var infoBlockBuilder = new TextBlockBuilder(Size.FullSize)
        {
            Background = Color.Transparent,
            Foreground = Color.Default,
            StartingText = "Press Tab",
            TextHorizontalAligning = HorizontalAligning.Center,
            TextVerticalAligning = VerticalAligning.Center,
            WordWrap = true,
        };
        gridBuilder.Add(infoBlockBuilder, 0, 1);
        
        return gridBuilder;
    }

    private GridBuilder InitializeRootGridBuilder(Theme theme)
    {
        var gridRows = GridRowDefinition.From(
            new[] { GridRow.FromHeight(6), GridRow.FromRowRelation(1), });
        var gridColumns = GridColumnDefinition.From(1);
        var gridDefinition = new GridDefinition(gridRows, gridColumns);

        var rootGridBuilder = new GridBuilder(Size.FullSize, gridDefinition)
        {
            FocusChangeKeys = ImmutableList<ConsoleKey>.Empty,
            FocusUpKeys = ImmutableList.Create(ConsoleKey.Tab),
            FocusDownKeys = ImmutableList.Create(ConsoleKey.Tab),
            FocusRightKeys = ImmutableList<ConsoleKey>.Empty,
            FocusLeftKeys = ImmutableList<ConsoleKey>.Empty,
            FocusFlowLoop = false,
            OverridesFocusFlow = false,
            BorderKind = BorderKind.None
        };

        return rootGridBuilder;
    }

    public void InitializeView(UIElement element, ViewContextSide context)
    {
        var messageBlock = _builtMessageBlock?.Element ?? throw new InvalidOperationException("Why is it not built?");
        var newTaskButton = _builtNewTaskButton?.Element ?? throw new InvalidOperationException("Why is it not built?");
        var newTaskDescriptionBox = _builtNewTaskDescriptionBox?.Element ?? throw new InvalidOperationException("Why is it not built?");
        var newTaskNameBox = _builtNewTaskNameBox?.Element ?? throw new InvalidOperationException("Why is it not built?");
        var tasksTable = _builtTasksTable?.Element ?? throw new InvalidOperationException("Why is it not built?");

        var dataTable = new BindableDataTable<string?>(MaxTodayTasks, 3, null);
        for (int row = 0; row < dataTable.RowCount; row++)
        {
            dataTable[row, 0] = $"{row + 1}";
        }
        
        for (int row = 0; row < context.Tasks.Count; row++)
        {
            var task = context.Tasks[row];
            dataTable[row, 1] = task.Name;
            dataTable[row, 2] = task.Description;
        }
        
        tasksTable.Observe(dataTable);
        
        SubscribeNewTaskButton(newTaskButton, newTaskNameBox, newTaskDescriptionBox, dataTable, messageBlock, context);
    }

    private void SubscribeNewTaskButton(Button button, TextBox nameBox, TextBox descriptionBox, 
        BindableDataTable<string?> dataTable, TextBlock messageBlock, ViewContextSide context)
    {
        button.Pressed += (sender, args) =>
        {
            if (string.IsNullOrWhiteSpace(nameBox.Text))
            {
                messageBlock.Text = "Name must be not empty";
                return;
            }

            var todayTasksCount = context.Tasks.GetToday().Length;

            if (todayTasksCount >= MaxTodayTasks)
            {
                messageBlock.Text = "Too many tasks today";
                return;
            }

            var taskName = nameBox.Text;
            var taskDescription = descriptionBox.Text;
            var taskId = context.Tasks.GenerateUniqueTaskId();
            var newTask = new ToDoTask(taskId, taskName, taskDescription, DateOnly.FromDateTime(DateTime.Today));
            
            dataTable[todayTasksCount, 1] = newTask.Name;
            dataTable[todayTasksCount, 2] = newTask.Description;

            context.AddTask(newTask);
            messageBlock.Text = "Successfully added";
        };
    }
}