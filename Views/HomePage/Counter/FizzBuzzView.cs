namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public static class FizzBuzzView
{
    public static Control Build(CounterState props) =>
        WithReactive((disposables) =>
            props.Count
                .Select(count => count switch
                {
                    0 => null,
                    _ when count % 15 == 0 => "FizzBuzz",
                    _ when count % 3 == 0 => "Fizz",
                    _ when count % 5 == 0 => "Buzz",
                    _ => null
                })
                .ToView(text =>
                    text switch
                    {
                        { } result => TextBlock()
                            .Text(result)
                            .FontSize(24)
                            .FontWeight(FontWeight.Bold)
                            .Foreground(Brushes.Orange)
                            .Margin(0, 20, 0, 0)
                            .HorizontalAlignment(HorizontalAlignment.Center),
                        _ => null
                    })
            );
}
