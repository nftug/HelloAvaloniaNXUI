namespace System.Reactive;

public static class CompositeDisposablesExtensions
{
    public static T AddTo<T>(this T disposable, CompositeDisposable compositeDisposable)
        where T : IDisposable
    {
        compositeDisposable.Add(disposable);
        return disposable;
    }
}
