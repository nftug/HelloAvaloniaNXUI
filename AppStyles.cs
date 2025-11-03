namespace HelloAvaloniaNXUI;

public static class AppStyles
{
    public static Styles Build()
    {
        var styles = new Styles();

        styles.AddRange([
            Style(x => x.OfType<Button>())
                .Setter(Avalonia.Controls.ContentControl.HorizontalContentAlignmentProperty, HorizontalAlignment.Center)
                .Setter(Avalonia.Controls.ContentControl.VerticalContentAlignmentProperty, VerticalAlignment.Center),
            Style(x => x.OfType<ToggleButton>())
                .Setter(Avalonia.Controls.ContentControl.HorizontalContentAlignmentProperty, HorizontalAlignment.Center)
                .Setter(Avalonia.Controls.ContentControl.VerticalContentAlignmentProperty, VerticalAlignment.Center)
        ]);

        return styles;
    }
}
