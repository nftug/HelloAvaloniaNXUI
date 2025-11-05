namespace HelloAvaloniaNXUI.Utils;

public static class ObservableExtensions
{
    public static ReadOnlyReactiveProperty<T> ToReactiveValue<T>(
        this Observable<T> source, CompositeDisposable disposables, T initialValue = default!)
    {
        return source.DistinctUntilChanged().ToReadOnlyReactiveProperty(initialValue).AddTo(disposables);
    }

    public static Observable<U> MapFromCollectionChanged<TObs, T, U>(
        this ObservableCollection<TObs> collection,
        Func<T[], U> selector,
        U defaultValue,
        bool startImmediately = true)
        where TObs : Observable<T>
    {
        var trigger = collection.ObservePropertyChanged(c => c.Count);
        if (startImmediately) trigger = trigger.Prepend(collection.Count);

        return trigger.SelectMany(count =>
            count == 0
                ? Observable.Return(defaultValue)
                : Observable.CombineLatest(collection).Select(selector));
    }

    public static Observable<U> MapFromCollectionChanged<T, U>(
        this ObservableCollection<ReactiveProperty<T>> collection,
        Func<T[], U> selector,
        U defaultValue = default!,
        bool startImmediately = true)
        => MapFromCollectionChanged<ReactiveProperty<T>, T, U>(collection, selector, defaultValue, startImmediately);

    public static Observable<U> MapFromCollectionChanged<T, U>(
        this ObservableCollection<Observable<T>> collection,
        Func<T[], U> selector,
        U defaultValue,
        bool startImmediately = true)
        => MapFromCollectionChanged<Observable<T>, T, U>(collection, selector, defaultValue, startImmediately);

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
