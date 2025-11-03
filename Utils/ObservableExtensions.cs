namespace HelloAvaloniaNXUI.Utils;

public static class ObservableExtensions
{
    public static ReadOnlyReactiveProperty<T> ToReadOnly<T>(
        this Observable<T> source, R3.CompositeDisposable disposables, T initialValue = default!)
    {
        return source.ToReadOnlyReactiveProperty(initialValue).ToReadOnlyReactiveProperty().AddTo(disposables);
    }
}
