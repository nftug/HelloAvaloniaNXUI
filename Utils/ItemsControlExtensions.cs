namespace HelloAvaloniaNXUI.Utils;

public static class ItemsControlExtensions
{
    public static ItemsControl ItemTemplateObservable<TObs, T>(
        this ItemsControl control,
        Func<TObs, Control?> render,
        bool supportsRecycling = true
    )
        where TObs : Observable<T>
    {
        var template = new FuncDataTemplate<TObs>((value, _) => render(value), supportsRecycling);
        return control.ItemTemplate(template);
    }

    public static ItemsControl ItemTemplateObservable<T>(
        this ItemsControl control,
        Func<Observable<T>, Control?> render,
        bool supportsRecycling = true
    )
        => ItemTemplateObservable<Observable<T>, T>(control, render, supportsRecycling);

    public static ItemsControl ItemTemplateObservable<T>(
        this ItemsControl control,
        Func<ReactiveProperty<T>, Control?> render,
        bool supportsRecycling = true
    )
        => ItemTemplateObservable<ReactiveProperty<T>, T>(control, render, supportsRecycling);
}
