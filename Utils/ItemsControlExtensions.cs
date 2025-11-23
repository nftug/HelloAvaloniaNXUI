namespace HelloAvaloniaNXUI.Utils;

public static class ItemsControlExtensions
{
    public static ItemsControl ItemTemplate<T>(
        this ItemsControl control,
        Func<T, Control?> render,
        bool supportsRecycling = true
    )
    {
        var template = new FuncDataTemplate<T>((value, _) => render(value), supportsRecycling);
        return control.ItemTemplate(template);
    }
}
