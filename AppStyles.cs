namespace HelloAvaloniaNXUI;

public class AppStyles : Styles
{
    public AppStyles()
    {
        AddRange([
                Style(x => x.OfType<Button>())
                    .SetContentControlHorizontalContentAlignment(HorizontalAlignment.Center)
                    .SetContentControlVerticalContentAlignment(VerticalAlignment.Center),
                Style(x => x.OfType<ToggleButton>())
                    .SetTextBoxHorizontalContentAlignment(HorizontalAlignment.Center)
                    .SetTextBoxVerticalContentAlignment(VerticalAlignment.Center),
            ]);
    }
}
