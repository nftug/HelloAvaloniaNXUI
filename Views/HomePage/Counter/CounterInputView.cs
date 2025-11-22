namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public static class CounterInputView
{
    public static Control Build() =>
        WithLifecycle((disposables, control) =>
        {
            var (state, ctxDisposables) = CounterContext.Require(control);

            var count = state.Count.ToReactiveValue(disposables);

            var inputCount = new ReactiveProperty<decimal?>(count.CurrentValue).AddTo(disposables);

            count.Subscribe(c => inputCount.Value = c).AddTo(disposables);

            var canSetInput = state.IsSetting
                .CombineLatest(
                    count,
                    inputCount,
                    (isSetting, current, input) => !isSetting && input != current
                );

            async void HandleSetInput() =>
                await InvokeAsync(ctxDisposables,
                    async ct =>
                    {
                        if (inputCount.Value is { } value)
                            await state.SetCountAsync((int)value, TimeSpan.FromSeconds(0.5), ct);
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
                        .IsEnabled(state.IsSetting.Select(v => !v).AsSystemObservable())
                        .Margin(5.0, 0.0)
                        .VerticalAlignmentCenter(),
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
