namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public record CounterState(
    Observable<int> Count,
    Observable<bool> IsSetting,
    Func<int, TimeSpan, CancellationToken, Task> SetCountAsync
);

public static class CounterHooks
{
    private static async Task<int> FetchDelayedCountAsync(int newCount, TimeSpan delay, CancellationToken ct)
    {
        await Task.Delay(delay, ct);
        return newCount;
    }

    public static CounterState UseDelayedCounter(R3.CompositeDisposable disposables)
    {
        var count = new ReactiveProperty<int>(0).AddTo(disposables);
        var isSetting = new ReactiveProperty<bool>(false).AddTo(disposables);

        async Task SetCountAsync(int newCount, TimeSpan delay, CancellationToken ct)
        {
            try
            {
                isSetting.Value = true;
                count.Value = await FetchDelayedCountAsync(newCount, delay, ct);
            }
            finally
            {
                isSetting.Value = false;
            }
        }

        return new CounterState(count, isSetting, SetCountAsync);
    }
}
