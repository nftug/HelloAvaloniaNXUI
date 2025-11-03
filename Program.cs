using Avalonia.Controls.Notifications;
using DialogHostAvalonia;
using HelloAvaloniaNXUI;
using HelloAvaloniaNXUI.Views;
using Material.Icons;
using Material.Icons.Avalonia;

static Window Build()
{
    var mainPage = new PageItem(
        MaterialIconKind.Home,
        "Main",
        StackPanel()
            .Margin(20)
            .Spacing(20)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .VerticalAlignment(VerticalAlignment.Center)
            .Children(
                ClockView.Build(),
                CounterView.Build()
            )
    );

    var settingsPage = new PageItem(
        MaterialIconKind.Settings,
        "Settings",
        TextBlock()
            .Text("Settings Page")
            .FontSize(24)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .VerticalAlignment(VerticalAlignment.Center)
    );

    var aboutPage = new PageItem(
        MaterialIconKind.Information,
        "About",
        TextBlock()
            .Text("About Page")
            .FontSize(24)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .VerticalAlignment(VerticalAlignment.Center)
    );

    return Window()
        .Title("Hello Avalonia NXUI").Width(800).Height(600)
        .Styles(AppStyles.Build())
        .Styles(new DialogHostStyles())
        .Styles(new MaterialIconStyles(null))
        .Content(
            Grid()
                .Children(
                    NavigationView.Build(new([mainPage, settingsPage, aboutPage])),
                    WindowNotificationManager()
                        .Position(NotificationPosition.BottomCenter)
                        .MaxItems(1),
                    new DialogHost()));
}

AppBuilder.Configure<Application>()
  .UsePlatformDetect()
  .UseFluentTheme()
  .WithApplicationName("HelloAvaloniaNXUI")
  .StartWithClassicDesktopLifetime(Build, args);
