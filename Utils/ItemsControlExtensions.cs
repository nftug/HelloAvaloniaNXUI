namespace HelloAvaloniaNXUI.Utils;

public static class ItemsControlExtensions
{
    public static ItemsControl ItemTemplateFunc<T>(
        this ItemsControl control,
        Func<T, Control?> render,
        bool supportsRecycling = true
    )
    {
        var template = new FuncDataTemplate<T>((value, _) => render(value), supportsRecycling);
        return control.ItemTemplate(template);
    }

    public static ItemsControl TemplateFunc(this ItemsControl control, TemplatedControl templatedControl)
    {
        var template = new FuncControlTemplate<ItemsControl>((_, _) => templatedControl);
        return control.Template(template);
    }

    public static ItemsControl ItemsPanelFunc(this ItemsControl control, Panel panel)
    {
        return control.ItemsPanel(new FuncTemplate<Panel?>(() => panel));
    }

    public static ItemsControl ItemsSourceObservable<T>(
        this ItemsControl control,
        ObservableList<T> itemsSource)
    {
        return control.ItemsSource(itemsSource.ToNotifyCollectionChangedSlim());
    }
}
