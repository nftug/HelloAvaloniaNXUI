namespace HelloAvaloniaNXUI;

using Avalonia.Controls;

public static class AppStyles
{
    public static Styles Build()
    {
        var styles = new Styles();

        styles.AddRange([
            Style(x => x.OfType<Button>())
                .Setter(ContentControl.HorizontalContentAlignmentProperty, HorizontalAlignment.Center)
                .Setter(ContentControl.VerticalContentAlignmentProperty, VerticalAlignment.Center),
            Style(x => x.OfType<ToggleButton>())
                .Setter(ContentControl.HorizontalContentAlignmentProperty, HorizontalAlignment.Center)
                .Setter(ContentControl.VerticalContentAlignmentProperty, VerticalAlignment.Center)
        ]);

        return styles;
    }
}
