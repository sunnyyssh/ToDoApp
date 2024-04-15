using System.Collections.Immutable;
using Sunnyyssh.ConsoleUI;
using ToDoApp.ViewModel;

namespace ToDoApp.View;

public sealed class ToDoAppBuilder
{
    private readonly ViewContextSide _context;

    private readonly List<IViewBuilder> _views = new();

    public ToDoAppBuilder AddView(IViewBuilder viewBuilder)
    {
        ArgumentNullException.ThrowIfNull(viewBuilder, nameof(viewBuilder));
        if (_views.Contains(viewBuilder))
            throw new ArgumentException("Already added.", nameof(viewBuilder));
        
        _views.Add(viewBuilder);

        return this;
    }
    
    public Application Build()
    {
        var appBuilder = InitializeBuilder();

        var (builtChooser, switcherBuilder, builtSwitcher) 
            = WithBaseUI(appBuilder);
        var views = WithViews(switcherBuilder);

        var resultApp = appBuilder.Build();
        
        SubscribeChooser(builtChooser, builtSwitcher);
        InitializeViews(views);

        return resultApp;
    }

    private void SubscribeChooser(BuiltUIElement<OptionChooser> builtChooser, BuiltUIElement<UIElementSwitcher> builtSwitcher)
    {
        var menuChooser = builtChooser.Element ?? throw new InvalidOperationException("Why is it not built?");
        var switcher = builtSwitcher.Element ?? throw new InvalidOperationException("Why is it not built?");

        menuChooser.ChosenOn += (_, args) => switcher.SwitchTo(args.OptionIndex);
    }

    private void InitializeViews(IReadOnlyList<(IViewBuilder, BuiltUIElement)> views)
    {
        foreach (var (viewBuilder, builtView) in views)
        {
            var viewElement = builtView.Element ?? throw new InvalidOperationException("Why is it not built?");
            
            viewBuilder.InitializeView(viewElement, _context);
        }
    }

    private (BuiltUIElement<OptionChooser>, UIElementSwitcherBuilder, BuiltUIElement<UIElementSwitcher>) 
        WithBaseUI(ApplicationBuilder appBuilder)
    {
        var gridColumns = GridColumnDefinition.From(
            new[] { GridColumn.FromWidth(15), GridColumn.FromColumnRelation(1), });
        var gridRows = GridRowDefinition.From(
            new[] { GridRow.FromRowRelation(1), GridRow.FromHeight(1), });
        var gridDefinition = new GridDefinition(gridRows, gridColumns);

        var baseGridBuilder = new GridBuilder(Size.FullSize, gridDefinition)
        {
            BorderKind = BorderKind.SingleLine,
            BorderColor = _context.Settings.Theme.MenuBorderColor,
            FocusChangeKeys = ImmutableList<ConsoleKey>.Empty,
            FocusUpKeys = ImmutableList<ConsoleKey>.Empty,
            FocusDownKeys = ImmutableList<ConsoleKey>.Empty,
            FocusRightKeys = ImmutableList.Create(ConsoleKey.RightArrow),
            FocusLeftKeys = ImmutableList.Create(ConsoleKey.LeftArrow),
            OverridesFocusFlow = false,
        };

        baseGridBuilder.Add(InitializeDateBlockBuilder(), 1, 0)
            .Add(InitializeInfoBlockBuilder(), 1, 1);

        var chooserBuilder = InitializeMenuChooserBuilder();
        baseGridBuilder.Add(chooserBuilder, 0, 0, out var builtChooser);

        var switcherBuilder = InitializeSwitcherBuilder();
        baseGridBuilder.Add(switcherBuilder, 0, 1, out var builtSwitcher);

        appBuilder.Add(baseGridBuilder, Position.LeftTop);
        
        return (builtChooser, switcherBuilder, builtSwitcher);
    }

    private UIElementSwitcherBuilder InitializeSwitcherBuilder()
    {
        return new UIElementSwitcherBuilder(Size.FullSize)
        {
            OverridesFlow = false
        };
    }

    private IUIElementBuilder<OptionChooser> InitializeMenuChooserBuilder()
    {
        var theme = _context.Settings.Theme;
        
        var options = _views.Select(view => view.Name).ToArray();
        const int optionHeight = 3;
        int height = optionHeight * options.Length;

        var colorSet = new TextOptionColorSet(theme.MenuNotFocusedBackground, Color.Default)
        {
            ChosenBackground = theme.MenuChosenBackground,
            FocusedBackground = theme.MenuFocusedBackground,
            ChosenFocusedBackground = theme.MenuChosenFocusedBackground
        };
        
        var chooserBuilder = new RowTextChooserBuilder(1.0, height, Orientation.Vertical, options)
        {
            BorderKind = BorderKind.None,
            CanChooseOnlyOne = true,
            ColorSet = colorSet,
        };

        return chooserBuilder;
    }

    private TextBlockBuilder InitializeDateBlockBuilder()
    {
        var content = DateTime.Today.ToString("MM/dd/yyyy");
        
        var dateBlockBuilder = new TextBlockBuilder(Size.FullSize)
        {
            Background = Color.Transparent,
            Foreground = Color.Default,
            TextHorizontalAligning = HorizontalAligning.Center,
            TextVerticalAligning = VerticalAligning.Center,
            WordWrap = false,
            StartingText = content
        };

        return dateBlockBuilder;
    }
    
    private TextBlockBuilder InitializeInfoBlockBuilder()
    {
        var content = $"Press Left or Right to move between menu and content window";
            
        var infoBlockBuilder = new TextBlockBuilder(Size.FullSize)
        {
            Background = Color.Transparent,
            Foreground = Color.Default,
            TextHorizontalAligning = HorizontalAligning.Center,
            TextVerticalAligning = VerticalAligning.Center,
            WordWrap = false,
            StartingText = content
        };

        return infoBlockBuilder;
    }

    private IReadOnlyList<(IViewBuilder, BuiltUIElement)> WithViews(UIElementSwitcherBuilder switcherBuilder)
    {
        var result = new (IViewBuilder, BuiltUIElement)[_views.Count];

        for (int i = 0; i < _views.Count; i++)
        {
            var viewBuilder = _views[i].InitializeBuilder(_context);
            switcherBuilder.Add(viewBuilder, out var builtView);
            result[i] = (_views[i], builtView);
        }

        return result;
    }

    private ApplicationBuilder InitializeBuilder()
    {
        var settings = _context.Settings;

        var appSettings = new ApplicationSettings()
        {
            DefaultBackground = settings.Theme.DefaultBackground,
            DefaultForeground = settings.Theme.DefaultForeground,
            EnableOverlapping = true,
            FocusChangeKeys = ImmutableList<ConsoleKey>.Empty,
            BorderConflictsAllowed = false,
            KillApplicationKey = ConsoleKey.Escape,
        };

        return new ApplicationBuilder(appSettings);
    }
    
    public ToDoAppBuilder(ViewContextSide context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _context = context;
    }
}