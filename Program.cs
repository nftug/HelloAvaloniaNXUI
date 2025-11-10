using DialogHostAvalonia;
using HelloAvaloniaNXUI;
using HelloAvaloniaNXUI.Views.AboutPage;
using HelloAvaloniaNXUI.Views.Common;
using HelloAvaloniaNXUI.Views.CounterListPage;
using HelloAvaloniaNXUI.Views.HomePage;
using HelloAvaloniaNXUI.Views.HomePage.Counter;
using Material.Icons.Avalonia;

Window Build() =>
    Window()
        .Title("Hello Avalonia NXUI").Width(1024).Height(680)
        .WindowStartupLocationCenterScreen()
        .Styles(new AppStyles())
        .Styles(new DialogHostStyles())
        .Styles(new MaterialIconStyles(null))
        .AppTrayIcon()
        .Content(
            Grid().Children(
                CounterContextProvider.Build(
                    NavigationView.Build(new(
                        new Dictionary<string, PageItem>
                        {
                            ["/home"] = HomePageView.BuildPageItem(),
                            ["/counter-list"] = CounterListPage.BuildPageItem(),
                            ["/about"] = AboutPage.BuildPageItem(),
                        },
                        "/home"
                    ))
                ),
                WindowNotificationManager().PositionBottomCenter().MaxItems(1),
                new DialogHost()
            )
        );

AppBuilder.Configure<Application>()
  .UsePlatformDetect()
  .UseFluentTheme()
  .UseR3()
  .WithApplicationName("HelloAvaloniaNXUI")
  .StartWithClassicDesktopLifetime(Build, args);
