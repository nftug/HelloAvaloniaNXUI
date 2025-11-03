using Avalonia.Controls.Notifications;
using DialogHostAvalonia;
using HelloAvaloniaNXUI;
using HelloAvaloniaNXUI.Views;

static Window Build() =>
    Window()
        .Title("Hello Avalonia NXUI").Width(800).Height(600)
        .Styles(AppStyles.Build())
        .Styles(new DialogHostStyles())
        .Content(
            Grid()
                .Children(
                    WindowNotificationManager()
                        .Position(NotificationPosition.BottomCenter)
                        .MaxItems(1),
                    StackPanel()
                        .Margin(20)
                        .Spacing(20)
                        .HorizontalAlignment(HorizontalAlignment.Center)
                        .VerticalAlignment(VerticalAlignment.Center)
                        .Children(
                            ClockView.Build(),
                            CounterView.Build()
                        ),
                    new DialogHost()));

AppBuilder.Configure<Application>()
  .UsePlatformDetect()
  .UseFluentTheme()
  .WithApplicationName("HelloAvaloniaNXUI")
  .StartWithClassicDesktopLifetime(Build, args);
