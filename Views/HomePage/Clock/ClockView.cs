namespace HelloAvaloniaNXUI.Views.HomePage.Clock;

public sealed record ClockState(Observable<DateTime> CurrentTime);

public static class ClockHooks
{
    private static readonly Observable<DateTime> _timeObservable =
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(_ => DateTime.Now)
            .Publish()
            .RefCount();

    public static ClockState UseClock(CompositeDisposable disposables)
    {
        var currentTime = new ReactiveProperty<DateTime>(DateTime.Now).AddTo(disposables);

        _timeObservable.Subscribe(t => currentTime.Value = t).AddTo(disposables);

        return new ClockState(currentTime);
    }
}

public static class ClockView
{
    public static Control Build() =>
        WithLifecycle((disposables, _) =>
        {
            var clockState = ClockHooks.UseClock(disposables);
            return TextBlock()
                .FontSize(56)
                .Margin(10)
                .HorizontalAlignmentCenter()
                .Text(clockState.CurrentTime
                    .Select(t => t.ToString("HH:mm:ss"))
                    .AsSystemObservable());
        });
}
