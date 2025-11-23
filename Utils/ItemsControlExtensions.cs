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

    public static ItemsControl Template(this ItemsControl control, TemplatedControl templatedControl)
    {
        var template = new FuncControlTemplate<ItemsControl>((_, _) => templatedControl);
        return control.Template(template);
    }

    public static ItemsControl ItemsPanel(this ItemsControl control, Panel panel)
    {
        control.ItemsPanel = new FuncTemplate<Panel?>(() => panel);
        return control;
    }
}
