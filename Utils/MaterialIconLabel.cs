using Material.Icons;
using Material.Icons.Avalonia;

namespace HelloAvaloniaNXUI.Utils;

public static class MaterialIconLabel
{
    public static StackPanel Build(MaterialIconKind iconKind, string text) =>
        StackPanel()
            .OrientationHorizontal()
            .Spacing(10)
            .Children(
                new MaterialIcon { Kind = iconKind, FontSize = 20, VerticalAlignment = VerticalAlignment.Center },
                TextBlock().Text(text).FontSize(16).VerticalAlignmentCenter()
            );
}
