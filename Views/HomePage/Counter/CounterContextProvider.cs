namespace HelloAvaloniaNXUI.Views.HomePage.Counter;

public static class CounterContext
{
    public static Control Build(Control content, string? name = null) =>
        ContextProvider<CounterState>.Provide(CounterHooks.UseDelayedCounter, content, name);

    public static (CounterState value, CompositeDisposable ctxDisposables) Require(
        Control control, string? name = null) =>
        ContextProvider<CounterState>.Require(control, name);
}
