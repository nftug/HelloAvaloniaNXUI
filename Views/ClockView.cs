using System.Reactive;
using Reactive.Bindings;

namespace HelloAvaloniaNXUI.Views;

public sealed record ClockState(IObservable<DateTime> CurrentTime);

public static class ClockHooks
{
    private static readonly IObservable<DateTime> _timeObservable =
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(_ => DateTime.Now)
            .Publish()
            .RefCount();

    public static ClockState UseClock(CompositeDisposable disposables)
    {
        var currentTime = new ReactivePropertySlim<DateTime>(DateTime.Now).AddTo(disposables);

        _timeObservable.Subscribe(t => currentTime.Value = t).AddTo(disposables);

        return new ClockState(currentTime);
    }
}

public static class ClockView
{
    public static Control Build()
    {
        var disposables = new CompositeDisposable();

        var clockState = ClockHooks.UseClock(disposables);

        return new TextBlock()
            .Text(clockState.CurrentTime.Select(t => t.ToString("HH:mm:ss")))
            .FontSize(56)
            .Margin(10)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .OnDetached(disposables.Dispose);
    }
}
