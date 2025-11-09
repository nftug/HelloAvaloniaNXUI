using HelloAvaloniaNXUI.Views.Common;
using HelloAvaloniaNXUI.Views.HomePage.Clock;
using HelloAvaloniaNXUI.Views.HomePage.Counter;
using Material.Icons;

namespace HelloAvaloniaNXUI.Views.HomePage;

public static class HomePageView
{
    private static StackPanel Build() =>
        StackPanel()
            .Margin(20)
            .Spacing(20)
            .HorizontalAlignmentCenter()
            .VerticalAlignmentCenter()
            .Children(
                ClockView.Build(),
                CounterView.Build()
            );

    public static PageItem BuildPageItem() =>
        new(MaterialIconKind.Home, "Home", Build());
}
