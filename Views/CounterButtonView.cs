using System.Reactive;
using Reactive.Bindings;

namespace HelloAvaloniaNXUI.Views;

public record CounterActionButtonViewProps(
    ReadOnlyReactivePropertySlim<int> Count,
    ReadOnlyReactivePropertySlim<bool> IsSetting,
    Func<int, TimeSpan, Task> SetCountAsync
);

public static class CounterActionButtonView
{
    public static Control Build(CounterActionButtonViewProps props)
    {
        var disposables = new CompositeDisposable();

        var canIncrement = props.IsSetting.Select(v => !v).ToReadOnlyReactivePropertySlim().AddTo(disposables);
        var canDecrement = props.IsSetting
            .CombineLatest(props.Count, (isSetting, count) => !isSetting && count > 0)
            .ToReadOnlyReactivePropertySlim()
            .AddTo(disposables);
        var canReset = props.IsSetting
            .CombineLatest(props.Count, (isSetting, count) => !isSetting && count != 0)
            .ToReadOnlyReactivePropertySlim()
            .AddTo(disposables);

        async void HandleIncrement(Control _)
        {
            if (!canIncrement.Value) return;
            await props.SetCountAsync(props.Count.Value + 1, TimeSpan.FromSeconds(0.1));
        }

        async void HandleDecrement(Control _)
        {
            if (!canDecrement.Value) return;
            await props.SetCountAsync(props.Count.Value - 1, TimeSpan.FromSeconds(0.1));
        }

        async void HandleReset(Control _)
        {
            if (!canReset.Value) return;
            await props.SetCountAsync(0, TimeSpan.FromSeconds(0.5));
        }

        return Grid()
            .Width(300)
            .ColumnDefinitions("1*, 1*")
            .RowDefinitions("Auto, Auto")
            .Children(
                Button()
                    .Content("Increment")
                    .OnClick(ControlEvent.Use<Button>(HandleIncrement, disposables))
                    .IsEnabled(canIncrement)
                    .Margin(new Thickness(5.0, 0.0))
                    .HorizontalAlignment(HorizontalAlignment.Stretch)
                    .Column(0),
                Button()
                    .Content("Decrement")
                    .OnClick(ControlEvent.Use<Button>(HandleDecrement, disposables))
                    .IsEnabled(canDecrement)
                    .Margin(new Thickness(5.0, 0.0))
                    .HorizontalAlignment(HorizontalAlignment.Stretch)
                    .Column(1),
                Button()
                    .Content("Reset")
                    .OnClick(ControlEvent.Use<Button>(HandleReset, disposables))
                    .IsEnabled(canReset)
                    .Margin(new Thickness(5.0, 10.0))
                    .HorizontalAlignment(HorizontalAlignment.Stretch)
                    .Column(0)
                    .ColumnSpan(2)
                    .Row(1)
            )
            .OnDetached(disposables.Dispose);
    }
}
