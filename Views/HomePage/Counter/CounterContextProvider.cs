namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public static class CounterContextProvider
{
    public static Control Build(Control content) =>
        Context<CounterState>.Provide(CounterHooks.UseDelayedCounter, content);
}
