using Sunnyyssh.ConsoleUI;
using ToDoApp.ViewModel;

namespace ToDoApp.View;

public class SettingsViewBuilder : IViewBuilder
{
    public string Name { get; } = "Settings";

    private BuiltUIElement<OptionChooser>? _themeChooserWaiter;
    
    public IUIElementBuilder InitializeBuilder(ViewContextSide context)
    {
        var theme = context.Settings.Theme;
        
        var rootStackBuilder = new StackPanelBuilder(Size.FullSize, Orientation.Vertical)
        {
            OverridesFocusFlow = false,
            FocusFlowLoop = false
        };

        var messageBlock = new TextBlockBuilder(1.0, 3)
        {
            Background = Color.Transparent,
            Foreground = theme.AdditionalForeground,
            StartingText = "Choose Theme. (It will be changed when app is restarted). Press L to switch to Light and D to switch to Dark and press Enter to apply.",
            TextHorizontalAligning = HorizontalAligning.Center,
            TextVerticalAligning = VerticalAligning.Center,
            WordWrap = true
        };
        rootStackBuilder.Add(messageBlock);

        var themeChooserBuilder = new RowTextChooserBuilder(1.0, 5, Orientation.Horizontal,
            new[] { Theme.Dark.HumanizedName, Theme.Light.HumanizedName })
        {
            BorderColor = theme.BorderColor,
            BorderKind = BorderKind.Dense,
            CanChooseOnlyOne = true,
            ColorSet = new TextOptionColorSet(Color.Transparent, Color.Default)
            {
                FocusedBackground = theme.MenuFocusedBackground,
                ChosenBackground = theme.MenuChosenBackground,
                ChosenFocusedBackground = theme.MenuChosenBackground
            },
            KeySet = new OptionChooserKeySet(
                new[] { ConsoleKey.L },
                new[] { ConsoleKey.D },
                new[] { ConsoleKey.Enter },
                Array.Empty<ConsoleKey>()),
        };
        rootStackBuilder.Add(themeChooserBuilder, out _themeChooserWaiter);

        return rootStackBuilder;
    }

    public void InitializeView(UIElement element, ViewContextSide context)
    {
        if (_themeChooserWaiter?.Element is not {} chooser)
        {
            throw new InvalidOperationException("How is it not built?");
        }

        chooser.ChosenOn += (sender, args) =>
        {
            var newTheme = args.OptionIndex == 0 ? Theme.Dark : Theme.Light;
            var newSettings = new Settings(newTheme);
            context.UpdateSettings(newSettings);
        };
    }
}