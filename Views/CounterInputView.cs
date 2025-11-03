using System.Reactive;
using Reactive.Bindings;

namespace HelloAvaloniaNXUI.Views;

public record CounterInputViewProps(
    ReadOnlyReactivePropertySlim<int> Count,
    ReadOnlyReactivePropertySlim<bool> IsSetting,
    Func<int, TimeSpan, Task> SetCountAsync
);

public static class CounterInputView
{
    public static Control Build(CounterInputViewProps props)
    {
        var disposables = new CompositeDisposable();
        var inputCount = new ReactivePropertySlim<decimal?>(props.Count.Value).AddTo(disposables);

        props.Count
            .Subscribe(c => inputCount.Value = c)
            .AddTo(disposables);

        var canSetInput = props.IsSetting
            .CombineLatest(
                props.Count,
                inputCount,
                (isSetting, current, input) => !isSetting && input != current
            )
            .ToReadOnlyReactivePropertySlim()
            .AddTo(disposables);

        async void HandleSetInput(Control _)
        {
            if (!canSetInput.Value) return;
            if (inputCount.Value is null) return;
            await props.SetCountAsync((int)inputCount.Value, TimeSpan.FromSeconds(0.3));
        }

        return Grid()
            .ColumnDefinitions("1*, Auto")
            .Children(
                new NumericUpDown()
                    .Value(inputCount)
                    .Minimum(0)
                    .Maximum(10000)
                    .FormatString("0")
                    .OnValueChanged(ControlEvent.Use<NumericUpDown>((ctrl) =>
                    {
                        if (ctrl.Value is { } newValue) inputCount.Value = newValue;
                    }, disposables))
                    .IsEnabled(props.IsSetting.Select(v => !v))
                    .Margin(new Thickness(5.0, 0.0))
                    .VerticalAlignment(VerticalAlignment.Center),
                new Button()
                    .Content("Set")
                    .OnClick(ControlEvent.Use<Button>(HandleSetInput, disposables))
                    .IsEnabled(canSetInput)
                    .Margin(new Thickness(5.0, 0.0))
                    .Width(80)
                    .Column(1)
            )
            .OnDetached(disposables.Dispose);
    }
}
