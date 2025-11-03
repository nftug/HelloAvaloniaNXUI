namespace HelloAvaloniaNXUI;

public static class AppStyles
{
    public static Styles Build()
    {
        var styles = new Styles();

        styles.AddRange([
            Style(x => x.OfType<Button>())
                .Setter(Avalonia.Layout.Layoutable.HorizontalAlignmentProperty, HorizontalAlignment.Center)
                .Setter(Avalonia.Layout.Layoutable.VerticalAlignmentProperty, VerticalAlignment.Center),
            Style(x => x.OfType<ToggleButton>())
                .Setter(Avalonia.Layout.Layoutable.HorizontalAlignmentProperty, HorizontalAlignment.Center)
                .Setter(Avalonia.Layout.Layoutable.VerticalAlignmentProperty, VerticalAlignment.Center)
        ]);

        return styles;
    }
}
