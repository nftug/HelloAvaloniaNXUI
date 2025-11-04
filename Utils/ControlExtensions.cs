namespace HelloAvaloniaNXUI.Utils;

public static class ControlExtensions
{
    public static TControl OnDetached<TControl>(this TControl control, Action action)
         where TControl : Control
    {
        control.DetachedFromVisualTree += (_, _) => action();
        return control;
    }

    public static TControl OnAttached<TControl>(this TControl control, Action action)
        where TControl : Control
    {
        control.AttachedToVisualTree += (_, _) => action();
        return control;
    }
}
