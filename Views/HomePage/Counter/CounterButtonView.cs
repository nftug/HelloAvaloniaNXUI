using Avalonia.Controls.Notifications;
using HelloAvaloniaNXUI.Views.Common;
using Material.Icons;

namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public static class CounterActionButtonView
{
    public static Control Build() =>
        WithReactive((disposables, control) =>
        {
            var (state, ctxDisposables) = CounterContext.Require(control);

            var count = state.Count.ToReactiveValue(disposables);

            var canIncrement = state.IsSetting.Select(v => !v);

            var canDecrement = state.IsSetting
                .CombineLatest(count, (isSetting, count) => !isSetting && count > 0);

            var canReset = state.IsSetting
                .CombineLatest(count, (isSetting, count) => !isSetting && count != 0);

            async void HandleIncrement() =>
                await InvokeAsync(ctxDisposables,
                    ct => state.SetCountAsync(count.CurrentValue + 1, TimeSpan.FromSeconds(0.1), ct));

            async void HandleDecrement() =>
                await InvokeAsync(ctxDisposables,
                    ct => state.SetCountAsync(count.CurrentValue - 1, TimeSpan.FromSeconds(0.1), ct));

            async void HandleReset() =>
                await InvokeAsync(ctxDisposables,
                    async ct =>
                    {
                        var ans = await MessageBoxView.ShowAsync(
                            new("Reset Counter",
                                "Are you sure you want to reset the counter to zero?",
                                MessageBoxButton.OkCancel), ct);

                        if (!ans) return;

                        await state.SetCountAsync(0, TimeSpan.FromSeconds(3.0), ct);

                        NotificationView.Show(
                            "Counter Reset",
                            "The counter has been reset to zero.",
                            NotificationType.Information
                        );
                    });

            return Grid()
                .Width(300)
                .ColumnDefinitions("1*, 1*")
                .RowDefinitions("Auto, Auto")
                .Children(
                    Button()
                        .Content(MaterialIconLabel.Build(MaterialIconKind.Plus, "Increment"))
                        .OnClickHandler((_, _) => HandleIncrement())
                        .IsEnabled(canIncrement.AsSystemObservable())
                        .Margin(5.0, 0.0)
                        .HorizontalAlignmentStretch()
                        .Column(0),
                    Button()
                        .Content(MaterialIconLabel.Build(MaterialIconKind.Minus, "Decrement"))
                        .OnClickHandler((_, _) => HandleDecrement())
                        .IsEnabled(canDecrement.AsSystemObservable())
                        .Margin(5.0, 0.0)
                        .HorizontalAlignmentStretch()
                        .Column(1),
                    Button()
                        .Content("Reset")
                        .OnClickHandler((_, _) => HandleReset())
                        .IsEnabled(canReset.AsSystemObservable())
                        .Margin(5.0, 10.0)
                        .HorizontalAlignmentStretch()
                        .Column(0)
                        .ColumnSpan(2)
                        .Row(1)
                );
        });
}
