using HelloAvaloniaNXUI.Views.Common;
using Material.Icons;

namespace HelloAvaloniaNXUI.Views.AboutPage;

public static class AboutPage
{
    private static StackPanel Build() =>
        StackPanel()
            .Margin(20)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .VerticalAlignment(VerticalAlignment.Center)
            .Children(
                TextBlock()
                    .Text("Hello Avalonia NXUI")
                    .FontSize(32)
                    .FontWeight(FontWeight.Bold)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .Margin(0, 0, 0, 30),
                TextBlock()
                    .Text("A sample application build with Avalonia UI and NXUI.")
                    .TextWrapping(TextWrapping.Wrap)
                    .FontSize(16)
                    .HorizontalAlignment(HorizontalAlignment.Center)
            );

    public static PageItem BuildPageItem() =>
        new(MaterialIconKind.Information, "About", Build());
}
