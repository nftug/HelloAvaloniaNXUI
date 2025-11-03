namespace HelloAvaloniaNXUI.Utils;

public static class ControlExtensions
{
    public static TControl OnDetached<TControl>(this TControl control, Action action)
         where TControl : Control
    {
        control.DetachedFromVisualTree += (_, _) => action();
        return control;
    }
}
