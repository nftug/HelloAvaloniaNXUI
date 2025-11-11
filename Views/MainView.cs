using DialogHostAvalonia;
using HelloAvaloniaNXUI.Views.AboutPage;
using HelloAvaloniaNXUI.Views.Common;
using HelloAvaloniaNXUI.Views.CounterListPage;
using HelloAvaloniaNXUI.Views.HomePage;
using HelloAvaloniaNXUI.Views.HomePage.Counter;

namespace HelloAvaloniaNXUI.Views;

public static class MainView
{
    public static Control Build() =>
        Grid().Children(
                CounterContext.Build(
                    NavigationView.Build(new(
                        new Dictionary<string, PageItem>
                        {
                            ["/home"] = HomePageView.BuildPageItem(),
                            ["/counter-list"] = CounterListPageView.BuildPageItem(),
                            ["/about"] = AboutPageView.BuildPageItem(),
                        },
                        "/home"
                    ))
                ),
                WindowNotificationManager().PositionBottomCenter().MaxItems(1),
                new DialogHost()
            );
}
