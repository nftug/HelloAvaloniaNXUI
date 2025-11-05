namespace HelloAvaloniaNXUI.Utils;

public static class ObservableExtensions
{
    public static ReadOnlyReactiveProperty<T> ToReactiveValue<T>(
        this Observable<T> source, CompositeDisposable disposables, T initialValue = default!)
    {
        return source.DistinctUntilChanged().ToReadOnlyReactiveProperty(initialValue).AddTo(disposables);
    }

    public static Control ToView<T>(
        this Observable<T> source,
        Func<T, Control?> render,
        CompositeDisposable? disposables = null)
    {
        var container = ContentControl();

        var d = source.Subscribe(value =>
        {
            container.Content = render(value);
        });

        if (disposables != null)
            disposables.Add(d);
        else
            container.DetachedFromVisualTree += (_, _) => d.Dispose();

        return container;
    }
}
