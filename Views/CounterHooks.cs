namespace HelloAvaloniaNXUI.Views;

public record CounterState(
    ReadOnlyReactiveProperty<int> Count,
    ReadOnlyReactiveProperty<bool> IsSetting,
    Func<int, TimeSpan, Task> SetCountAsync
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

        var cts = new CancellationTokenSource();

        // 破棄時にキャンセル
        disposables.Add(R3.Disposable.Create(cts.Cancel));

        async Task SetCountAsync(int newCount, TimeSpan delay)
        {
            isSetting.Value = true;
            try
            {
                var delayedCount = await FetchDelayedCountAsync(newCount, delay, cts.Token);
                if (cts.IsCancellationRequested) return;
                if (!count.IsDisposed)
                    count.Value = delayedCount;
            }
            catch (OperationCanceledException)
            {
                // キャンセル時は握りつぶす
            }
            finally
            {
                if (!isSetting.IsDisposed)
                    isSetting.Value = false;
            }
        }

        return new CounterState(count, isSetting, SetCountAsync);
    }
}
