namespace HelloAvaloniaNXUI.Utils;

public static class ItemsControlExtensions
{
    public static ItemsControl ItemTemplateObservable<T>(
        this ItemsControl control,
        Func<Observable<T>, Control?> render,
        bool supportsRecycling = true
    )
    {
        var template = new FuncDataTemplate<Observable<T>>((value, _) => render(value), supportsRecycling);
        return control.ItemTemplate(template);
    }

    public static ItemsControl ItemTemplateReactiveProperty<T>(
        this ItemsControl control,
        Func<ReactiveProperty<T>, Control?> render,
        bool supportsRecycling = true
    )
    {
        var template = new FuncDataTemplate<ReactiveProperty<T>>((value, _) => render(value), supportsRecycling);
        return control.ItemTemplate(template);
    }
}
