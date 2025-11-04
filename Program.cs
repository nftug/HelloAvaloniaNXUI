using Avalonia.Controls.Notifications;
using DialogHostAvalonia;
using HelloAvaloniaNXUI;
using HelloAvaloniaNXUI.Views.AboutPage;
using HelloAvaloniaNXUI.Views.Common;
using HelloAvaloniaNXUI.Views.HomePage;
using HelloAvaloniaNXUI.Views.SettingsPage;
using Material.Icons.Avalonia;

static Window Build() =>
    Window()
        .Title("Hello Avalonia NXUI").Width(1024).Height(680)
        .WindowStartupLocation(WindowStartupLocation.CenterScreen)
        .Styles(AppStyles.Build())
        .Styles(new DialogHostStyles())
        .Styles(new MaterialIconStyles(null))
        .AppTrayIcon()
        .Content(
            Grid()
                .Children(
                    NavigationView.Build(
                        new([
                            HomePageView.BuildPageItem(),
                            SettingsPage.BuildPageItem(),
                            AboutPage.BuildPageItem()
                        ])
                    ),
                    WindowNotificationManager()
                        .Position(NotificationPosition.BottomCenter)
                        .MaxItems(1),
                    new DialogHost()
                )
        );

AppBuilder.Configure<Application>()
  .UsePlatformDetect()
  .UseFluentTheme()
  .WithApplicationName("HelloAvaloniaNXUI")
  .StartWithClassicDesktopLifetime(Build, args);
