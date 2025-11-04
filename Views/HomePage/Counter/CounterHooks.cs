namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public record CounterState(
    ReadOnlyReactiveProperty<int> Count,
    ReadOnlyReactiveProperty<bool> IsSetting,
    Func<int, TimeSpan, Task<bool>> SetCountAsync
);

public static class CounterHooks
{
    private static async Task<int> FetchDelayedCountAsync(int newCount, TimeSpan delay, CancellationToken token)
    {
        await Task.Delay(delay, token);
        return newCount;
    }

    public static CounterState UseDelayedCounter(R3.CompositeDisposable disposables)
    {
        var count = new ReactiveProperty<int>(0).AddTo(disposables);
        var isSetting = new ReactiveProperty<bool>(false).AddTo(disposables);

        Task<bool> SetCountAsync(int newCount, TimeSpan delay) =>
            DispatchAsync(disposables,
                async token =>
                {
                    isSetting.Value = true;
                    count.Value = await FetchDelayedCountAsync(newCount, delay, token);
                },
                () => isSetting.Value = false
            );

        return new CounterState(count, isSetting, SetCountAsync);
    }
}
