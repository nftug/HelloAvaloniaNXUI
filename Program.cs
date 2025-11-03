using Avalonia.Controls.Notifications;
using HelloAvaloniaNXUI;
using HelloAvaloniaNXUI.Views;

static Window Build()
{
    var ctx = new ViewContext();
    var window = Window()
        .Title("Hello Avalonia NXUI").Width(800).Height(600)
        .Styles(AppStyles.Build())
        .Content(
            Grid()
                .Children(
                    WindowNotificationManager(out var notificationManager)
                        .Position(NotificationPosition.BottomCenter)
                        .MaxItems(1),
                    StackPanel()
                        .Margin(20)
                        .Spacing(20)
                        .HorizontalAlignment(HorizontalAlignment.Center)
                        .VerticalAlignment(VerticalAlignment.Center)
                        .Children(
                            ClockView.Build(ctx),
                            CounterView.Build(ctx)
                        ))
                );
    ctx.Inject(notificationManager);
    return window;
}

AppBuilder.Configure<Application>()
  .UsePlatformDetect()
  .UseFluentTheme()
  .WithApplicationName("HelloAvaloniaNXUI")
  .StartWithClassicDesktopLifetime(Build, args);
