using Avalonia.Controls.Notifications;
using HelloAvaloniaNXUI.Views.Common;

namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public static class CounterActionButtonView
{
    public static Control Build(CounterState props) =>
        WithReactive((disposables) =>
        {
            var count = props.Count.ToReactiveValue(disposables);

            var canIncrement = props.IsSetting.Select(v => !v);

            var canDecrement = props.IsSetting
                .CombineLatest(count, (isSetting, count) => !isSetting && count > 0);

            var canReset = props.IsSetting
                .CombineLatest(count, (isSetting, count) => !isSetting && count != 0);

            async void HandleIncrement() =>
                await InvokeAsync(disposables,
                    ct => props.SetCountAsync(count.CurrentValue + 1, TimeSpan.FromSeconds(0.1), ct));

            async void HandleDecrement() =>
                await InvokeAsync(disposables,
                    ct => props.SetCountAsync(count.CurrentValue - 1, TimeSpan.FromSeconds(0.1), ct));

            async void HandleReset() =>
                await InvokeAsync(disposables,
                    async ct =>
                    {
                        var ans = await MessageBoxView.ShowAsync(
                            new("Reset Counter",
                                "Are you sure you want to reset the counter to zero?",
                                MessageBoxButton.OkCancel), ct);

                        if (!ans) return;

                        await props.SetCountAsync(0, TimeSpan.FromSeconds(3.0), ct);

                        ShowNotification(new Notification(
                            "Counter Reset",
                            "The counter has been reset to zero.",
                            NotificationType.Information
                        ));
                    });

            return Grid()
                .Width(300)
                .ColumnDefinitions("1*, 1*")
                .RowDefinitions("Auto, Auto")
                .Children(
                    Button()
                        .Content("Increment")
                        .OnClickHandler((_, _) => HandleIncrement())
                        .IsEnabled(canIncrement.AsSystemObservable())
                        .Margin(5.0, 0.0)
                        .HorizontalAlignment(HorizontalAlignment.Stretch)
                        .Column(0),
                    Button()
                        .Content("Decrement")
                        .OnClickHandler((_, _) => HandleDecrement())
                        .IsEnabled(canDecrement.AsSystemObservable())
                        .Margin(5.0, 0.0)
                        .HorizontalAlignment(HorizontalAlignment.Stretch)
                        .Column(1),
                    Button()
                        .Content("Reset")
                        .OnClickHandler((_, _) => HandleReset())
                        .IsEnabled(canReset.AsSystemObservable())
                        .Margin(5.0, 10.0)
                        .HorizontalAlignment(HorizontalAlignment.Stretch)
                        .Column(0)
                        .ColumnSpan(2)
                        .Row(1)
                );
        });
}
