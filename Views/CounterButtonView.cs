using Avalonia.Controls.Notifications;

namespace HelloAvaloniaNXUI.Views;

public static class CounterActionButtonView
{
    public static Control Build(CounterState props)
    {
        var disposables = new R3.CompositeDisposable();

        var canIncrement = props.IsSetting.Select(v => !v).ToReadOnly(disposables);

        var canDecrement = props.IsSetting
            .CombineLatest(props.Count, (isSetting, count) => !isSetting && count > 0)
            .ToReadOnly(disposables);

        var canReset = props.IsSetting
            .CombineLatest(props.Count, (isSetting, count) => !isSetting && count != 0)
            .ToReadOnly(disposables);

        async void HandleIncrement()
        {
            if (!canIncrement.CurrentValue) return;
            await props.SetCountAsync(props.Count.CurrentValue + 1, TimeSpan.FromSeconds(0.1));
        }

        async void HandleDecrement()
        {
            if (!canDecrement.CurrentValue) return;
            await props.SetCountAsync(props.Count.CurrentValue - 1, TimeSpan.FromSeconds(0.1));
        }

        async void HandleReset()
        {
            if (!canReset.CurrentValue) return;

            var ans = await ConfirmDialogView.ShowAsync(
                "Reset Counter",
                "Are you sure you want to reset the counter to zero?"
            );
            if (!ans) return;

            await props.SetCountAsync(0, TimeSpan.FromSeconds(0.5));

            ShowNotification(new Notification(
                "Counter Reset",
                "The counter has been reset to zero.",
                NotificationType.Information
            ));
        }

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
            )
            .OnDetached(disposables.Dispose);
    }
}
