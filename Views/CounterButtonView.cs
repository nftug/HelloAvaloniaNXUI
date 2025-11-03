namespace HelloAvaloniaNXUI.Views;

public record CounterActionButtonViewProps(
    ReadOnlyReactiveProperty<int> Count,
    ReadOnlyReactiveProperty<bool> IsSetting,
    Func<int, TimeSpan, Task> SetCountAsync
);

public static class CounterActionButtonView
{
    public static Control Build(CounterActionButtonViewProps props)
    {
        var disposables = new R3.CompositeDisposable();

        var canIncrement = props.IsSetting.Select(v => !v).ToReadOnlyReactiveProperty().AddTo(disposables);

        var canDecrement = props.IsSetting
            .CombineLatest(props.Count, (isSetting, count) => !isSetting && count > 0)
            .ToReadOnlyReactiveProperty()
            .AddTo(disposables);

        var canReset = props.IsSetting
            .CombineLatest(props.Count, (isSetting, count) => !isSetting && count != 0)
            .ToReadOnlyReactiveProperty()
            .AddTo(disposables);

        async void HandleIncrement(Control _)
        {
            if (!canIncrement.CurrentValue) return;
            await props.SetCountAsync(props.Count.CurrentValue + 1, TimeSpan.FromSeconds(0.1));
        }

        async void HandleDecrement(Control _)
        {
            if (!canDecrement.CurrentValue) return;
            await props.SetCountAsync(props.Count.CurrentValue - 1, TimeSpan.FromSeconds(0.1));
        }

        async void HandleReset(Control _)
        {
            if (!canReset.CurrentValue) return;
            await props.SetCountAsync(0, TimeSpan.FromSeconds(0.5));
        }

        return Grid()
            .Width(300)
            .ColumnDefinitions("1*, 1*")
            .RowDefinitions("Auto, Auto")
            .Children(
                Button()
                    .Content("Increment")
                    .OnClick(BindEvent<Button>(HandleIncrement, disposables))
                    .IsEnabled(canIncrement.AsSystemObservable())
                    .Margin(new Thickness(5.0, 0.0))
                    .HorizontalAlignment(HorizontalAlignment.Stretch)
                    .Column(0),
                Button()
                    .Content("Decrement")
                    .OnClick(BindEvent<Button>(HandleDecrement, disposables))
                    .IsEnabled(canDecrement.AsSystemObservable())
                    .Margin(new Thickness(5.0, 0.0))
                    .HorizontalAlignment(HorizontalAlignment.Stretch)
                    .Column(1),
                Button()
                    .Content("Reset")
                    .OnClick(BindEvent<Button>(HandleReset, disposables))
                    .IsEnabled(canReset.AsSystemObservable())
                    .Margin(new Thickness(5.0, 10.0))
                    .HorizontalAlignment(HorizontalAlignment.Stretch)
                    .Column(0)
                    .ColumnSpan(2)
                    .Row(1)
            )
            .OnDetached(disposables.Dispose);
    }
}
