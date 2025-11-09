using HelloAvaloniaNXUI.Views.Common;
using Material.Icons;

namespace HelloAvaloniaNXUI.Views.AboutPage;

public static class AboutPage
{
    private static StackPanel Build() =>
        StackPanel()
            .Margin(20)
            .HorizontalAlignmentCenter()
            .VerticalAlignmentCenter()
            .Children(
                TextBlock()
                    .Text("Hello Avalonia NXUI")
                    .FontSize(32)
                    .FontWeightBold()
                    .HorizontalAlignmentCenter()
                    .Margin(0, 0, 0, 30),
               TextBlock()
                    .Text("A sample application build with Avalonia UI and NXUI.")
                    .TextWrappingWrap()
                    .FontSize(16)
                    .HorizontalAlignmentCenter()
            );

    public static PageItem BuildPageItem() =>
        new(MaterialIconKind.Information, "About", Build());
}
