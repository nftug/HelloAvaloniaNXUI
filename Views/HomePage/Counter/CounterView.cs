namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public static class CounterView
{
    public static Control Build() =>
        WithLifecycle((disposables, container) =>
        {
            var (state, ctxDisposables) = CounterContext.Require(container);

            return StackPanel()
                .Margin(20)
                .Spacing(10)
                .HorizontalAlignmentCenter()
                .VerticalAlignmentCenter()
                .Children(
                    TextBlock()
                        .Text(state.Count.Select(c => $"Count: {c}").AsSystemObservable())
                        .FontSize(24)
                        .Margin(0, 0, 0, 20)
                        .HorizontalAlignmentCenter(),
                    CounterInputView.Build(),
                    CounterActionButtonView.Build(),
                    FizzBuzzView.Build()
                );
        });
}