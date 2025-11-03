namespace HelloAvaloniaNXUI;

public class AppStyles : Styles
{
    public AppStyles()
    {
        Add(new Style(x => x.OfType<Button>())
        {
            Setters =
            {
                new Setter(Avalonia.Controls.ContentControl.HorizontalContentAlignmentProperty, HorizontalAlignment.Center),
                new Setter(Avalonia.Controls.ContentControl.VerticalContentAlignmentProperty, VerticalAlignment.Center),
            }
        });

        Add(new Style(x => x.OfType<ToggleButton>())
        {
            Setters =
            {
                new Setter(Avalonia.Controls.ContentControl.HorizontalContentAlignmentProperty, HorizontalAlignment.Center),
                new Setter(Avalonia.Controls.ContentControl.VerticalContentAlignmentProperty, VerticalAlignment.Center),
            }
        });
    }
}
