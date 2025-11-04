using HelloAvaloniaNXUI.Views.Common;
using Material.Icons;

namespace HelloAvaloniaNXUI.Views.SettingsPage;

public static class SettingsPage
{
    private static TextBlock Build() =>
        TextBlock()
            .Text("Settings Page")
            .FontSize(32)
            .FontWeight(FontWeight.Bold)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .VerticalAlignment(VerticalAlignment.Center);

    public static PageItem BuildPageItem() =>
        new(MaterialIconKind.Settings, "Settings", Build());
}
