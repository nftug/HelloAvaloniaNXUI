using Material.Icons;

namespace HelloAvaloniaNXUI.Utils;

public static class MaterialIconLabel
{
    public static Label Build(MaterialIconKind iconKind, string text) =>
        Label()
            .Content(
                StackPanel()
                    .OrientationHorizontal()
                    .Spacing(10)
                    .Children(new CustomIcon(iconKind), TextBlock().Text(text)));
}
