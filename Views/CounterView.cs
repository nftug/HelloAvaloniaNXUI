namespace HelloAvaloniaNXUI.Views;

public static class CounterView
{
    public static Control Build()
    {
        var disposables = new R3.CompositeDisposable();

        var counterState = CounterHooks.UseDelayedCounter(disposables);

        return StackPanel()
            .Margin(20)
            .Spacing(10)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .VerticalAlignment(VerticalAlignment.Center)
            .Children(
                TextBlock()
                    .Text(counterState.Count.Select(c => $"Count: {c}").AsSystemObservable())
                    .FontSize(24)
                    .Margin(0, 0, 0, 20)
                    .HorizontalAlignment(HorizontalAlignment.Center),
                CounterInputView.Build(counterState),
                CounterActionButtonView.Build(counterState)
            )
            .OnDetached(disposables.Dispose);
    }
}
