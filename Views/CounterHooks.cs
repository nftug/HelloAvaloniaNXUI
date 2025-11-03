using System.Reactive;
using Reactive.Bindings;

namespace HelloAvaloniaNXUI.Views;

public record CounterState(
    ReadOnlyReactivePropertySlim<int> Count,
    ReadOnlyReactivePropertySlim<bool> IsSetting,
    Func<int, TimeSpan, Task> SetCountAsync
);

public static class CounterHooks
{
    private static async Task<int> FetchDelayedCountAsync(int newCount, TimeSpan delay)
    {
        await Task.Delay(delay);
        return newCount;
    }

    public static CounterState UseDelayedCounter(CompositeDisposable disposables)
    {
        var count = new ReactivePropertySlim<int>(0).AddTo(disposables);
        var isSetting = new ReactivePropertySlim<bool>(false).AddTo(disposables);

        async Task SetCountAsync(int newCount, TimeSpan delay)
        {
            isSetting.Value = true;
            var delayedCount = await FetchDelayedCountAsync(newCount, delay);
            count.Value = delayedCount;
            isSetting.Value = false;
        }

        return new CounterState(
            count.ToReadOnlyReactivePropertySlim(),
            isSetting.ToReadOnlyReactivePropertySlim(),
            SetCountAsync
        );
    }
}
