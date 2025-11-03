using HelloAvaloniaNXUI;
using HelloAvaloniaNXUI.Views;

static Window Build()
    => Window()
        .Title("Hello Avalonia NXUI").Width(800).Height(600)
        .Styles(AppStyles.Build())
        .Content(
            StackPanel()
                .Margin(20)
                .Spacing(20)
                .HorizontalAlignment(HorizontalAlignment.Center)
                .VerticalAlignment(VerticalAlignment.Center)
                .Children(
                    ClockView.Build(),
                    CounterView.Build()
                ));

AppBuilder.Configure<Application>()
  .UsePlatformDetect()
  .UseFluentTheme()
  .WithApplicationName("HelloAvaloniaNXUI")
  .StartWithClassicDesktopLifetime(Build, args);
