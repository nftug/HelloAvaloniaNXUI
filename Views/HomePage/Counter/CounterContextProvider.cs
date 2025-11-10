namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public static class CounterContextProvider
{
    public static Control Build(Control content) =>
        WithReactive((disposables, _) =>
        {
            var counterState = CounterHooks.UseDelayedCounter(disposables);
            return new Context<CounterState>(counterState, content);
        });
}
