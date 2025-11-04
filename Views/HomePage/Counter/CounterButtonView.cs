using Avalonia.Controls.Notifications;
using HelloAvaloniaNXUI.Views.Common;

namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public static class CounterActionButtonView
{
    public static Control Build(CounterState props) =>
        WithReactive((disposables) =>
        {
            var canIncrement = props.IsSetting.Select(v => !v).ToReadOnly(disposables);

            var canDecrement = props.IsSetting
                .CombineLatest(props.Count, (isSetting, count) => !isSetting && count > 0)
                .ToReadOnly(disposables);

            var canReset = props.IsSetting
                .CombineLatest(props.Count, (isSetting, count) => !isSetting && count != 0)
                .ToReadOnly(disposables);

            async void HandleIncrement() =>
                await DispatchAsync(disposables,
                    async token =>
                    {
                        if (!canIncrement.CurrentValue) return;
                        await props.SetCountAsync(props.Count.CurrentValue + 1, TimeSpan.FromSeconds(0.1));
                    });

            async void HandleDecrement() =>
                await DispatchAsync(disposables,
                    async token =>
                    {
                        if (!canDecrement.CurrentValue) return;
                        await props.SetCountAsync(props.Count.CurrentValue - 1, TimeSpan.FromSeconds(0.1));
                    });

            async void HandleReset() =>
                await DispatchAsync(disposables,
                    async token =>
                    {
                        if (!canReset.CurrentValue) return;

                        var ans = await ConfirmDialogView.ShowAsync(
                            "Reset Counter",
                            "Are you sure you want to reset the counter to zero?"
                        );
                        if (!ans) return;

                        bool success = await props.SetCountAsync(0, TimeSpan.FromSeconds(3.0));
                        if (!success) return;

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
