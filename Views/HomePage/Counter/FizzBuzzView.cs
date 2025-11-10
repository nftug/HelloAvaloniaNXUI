namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public static class FizzBuzzView
{
    public static Control Build() =>
        WithReactive((disposables, control) =>
        {
            var (state, ctxDisposables) = Context<CounterState>.Require(control);
            var inputText = new ReactiveProperty<string?>().AddTo(disposables);

            return state.Count
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
                        { } result =>
                            TextBlock()
                                .Text(text)
                                .FontSize(24)
                                .FontWeightBold()
                                .Foreground(Brushes.Orange)
                                .Margin(0, 18, 0, 0)
                                .HorizontalAlignmentCenter(),
                        _ =>
                            TextBox()
                                .Text(inputText.AsSystemObservable())
                                .OnTextChangedHandler((ctl, _) => inputText.Value = ctl.Text)
                                .Margin(0, 20, 0, 0)
                                .Width(200)
                                .HorizontalAlignmentCenter()
                                .VerticalContentAlignmentCenter()
                    });
        });
}
