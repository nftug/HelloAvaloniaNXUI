using R3;

namespace HelloAvaloniaNXUI.Views;

public sealed record ClockState(Observable<DateTime> CurrentTime);

public static class ClockHooks
{
    private static readonly Observable<DateTime> _timeObservable =
        R3.Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(_ => DateTime.Now)
            .Publish()
            .RefCount();

    public static ClockState UseClock(R3.CompositeDisposable disposables)
    {
        var currentTime = new ReactiveProperty<DateTime>(DateTime.Now).AddTo(disposables);

        _timeObservable.Subscribe(t => currentTime.Value = t).AddTo(disposables);

        return new ClockState(currentTime);
    }
}

public static class ClockView
{
    public static Control Build()
    {
        var disposables = new R3.CompositeDisposable();

        var clockState = ClockHooks.UseClock(disposables);

        return TextBlock()
            .Text(clockState.CurrentTime.Select(t => t.ToString("HH:mm:ss")).AsSystemObservable())
            .FontSize(56)
            .Margin(10)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .OnDetached(disposables.Dispose);
    }
}
