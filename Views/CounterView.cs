using R3;

namespace HelloAvaloniaNXUI.Views;

public static class CounterView
{
    public static Control Build()
    {
        var disposables = new R3.CompositeDisposable();

        var counterState = CounterHooks.UseDelayedCounter(disposables);

        return new StackPanel()
            .Margin(20)
            .Spacing(10)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .VerticalAlignment(VerticalAlignment.Center)
            .Children(
                new TextBlock()
                    .Text(counterState.Count.Select(c => $"Count: {c}").AsSystemObservable())
                    .FontSize(24)
                    .Margin(new Thickness(0, 0, 0, 20))
                    .HorizontalAlignment(HorizontalAlignment.Center),
                CounterInputView.Build(new(counterState.Count, counterState.IsSetting, counterState.SetCountAsync)),
                CounterActionButtonView.Build(new(counterState.Count, counterState.IsSetting, counterState.SetCountAsync))
            )
            .OnDetached(disposables.Dispose);
    }
}
