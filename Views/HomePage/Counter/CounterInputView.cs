namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public static class CounterInputView
{
    public static Control Build(CounterState props) =>
        WithReactive((disposables) =>
        {
            var inputCount = new ReactiveProperty<decimal?>(props.Count.CurrentValue).AddTo(disposables);

            props.Count
                .Subscribe(c => inputCount.Value = c)
                .AddTo(disposables);

            var canSetInput = props.IsSetting
                .CombineLatest(
                    props.Count,
                    inputCount,
                    (isSetting, current, input) => !isSetting && input != current
                )
                .ToReadOnly(disposables);

            async void HandleSetInput() =>
                await InvokeAsync(disposables,
                    async ct =>
                    {
                        if (!canSetInput.CurrentValue || inputCount.Value == null) return;
                        await props.SetCountAsync((int)inputCount.Value!, TimeSpan.FromSeconds(0.5), ct);
                    });

            return Grid()
                .ColumnDefinitions("1*, Auto")
                .Children(
                    NumericUpDown()
                        .Value(inputCount.AsSystemObservable())
                        .OnValueChangedHandler((_, e) => inputCount.Value = e.NewValue)
                        .Minimum(0)
                        .Maximum(10000)
                        .FormatString("0")
                        .IsEnabled(props.IsSetting.Select(v => !v).AsSystemObservable())
                        .Margin(5.0, 0.0)
                        .VerticalAlignment(VerticalAlignment.Center),
                    Button()
                        .Content("Set")
                        .OnClickHandler((_, _) => HandleSetInput())
                        .IsEnabled(canSetInput.AsSystemObservable())
                        .Margin(5.0, 0.0)
                        .Width(80)
                        .Column(1)
                );
        });
}
