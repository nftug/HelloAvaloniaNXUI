namespace HelloAvaloniaNXUI.Views;

public record CounterState(
    ReadOnlyReactiveProperty<int> Count,
    ReadOnlyReactiveProperty<bool> IsSetting,
    Func<int, TimeSpan, Task> SetCountAsync
);

public static class CounterHooks
{
    private static async Task<int> FetchDelayedCountAsync(int newCount, TimeSpan delay)
    {
        await Task.Delay(delay);
        return newCount;
    }

    public static CounterState UseDelayedCounter(R3.CompositeDisposable disposables)
    {
        var count = new ReactiveProperty<int>(0).AddTo(disposables);
        var isSetting = new ReactiveProperty<bool>(false).AddTo(disposables);

        async Task SetCountAsync(int newCount, TimeSpan delay)
        {
            try
            {
                isSetting.Value = true;
                var delayedCount = await FetchDelayedCountAsync(newCount, delay);
                count.Value = delayedCount;
            }
            finally
            {
                isSetting.Value = false;
            }
        }

        return new CounterState(count, isSetting, SetCountAsync);
    }
}
