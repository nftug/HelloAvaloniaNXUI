namespace NXUI;

public static class ControlExtensions
{
    public static TControl OnDetached<TControl>(this TControl control, Action action)
         where TControl : Control
    {
        control.DetachedFromVisualTree += (_, _) => action();
        return control;
    }
}

public static class ControlEvent
{
    public static Action<TControl, IObservable<RoutedEventArgs>> BindEvent<TControl>(
        Action<TControl> action, R3.CompositeDisposable disposables)
        where TControl : Interactive
    {
        return (control, events) =>
        {
            events.Subscribe(_ => action(control)).AddTo(disposables);
        };
    }
}
