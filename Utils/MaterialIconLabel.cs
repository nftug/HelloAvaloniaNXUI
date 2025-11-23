using Material.Icons;
using Material.Icons.Avalonia;

namespace HelloAvaloniaNXUI.Utils;

public static class MaterialIconLabel
{
    public static Label Build(MaterialIconKind iconKind, string text) =>
        Label()
            .Content(
                StackPanel()
                    .OrientationHorizontal()
                    .Spacing(10)
                    .Children(new MaterialIcon() { Kind = iconKind }, TextBlock().Text(text)));
}
