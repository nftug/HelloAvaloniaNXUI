namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public static class CounterView
{
    public static Control Build() =>
        WithReactive((disposables, container) =>
        {
            var state = ContextView<CounterState>.Require(container).Value;

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
                    CounterInputView.Build(state),
                    CounterActionButtonView.Build(),
                    FizzBuzzView.Build(state)
                );
        });
}