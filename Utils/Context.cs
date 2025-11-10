namespace HelloAvaloniaNXUI.Utils;

public class Context<T> : ContentControl where T : class
{
    public T Value { get; }

    public CompositeDisposable Disposables { get; private set; } = [];

    public Context(T value, Control content)
    {
        Value = value;
        Content = content;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        Disposables = [];
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        Disposables.Dispose();
        Disposables = [];
        (Value as IDisposable)?.Dispose();
    }

    public static (T? value, CompositeDisposable? disposables) Resolve(Control control)
    {
        var context = control.FindAncestorOfType<Context<T>>();
        return (context?.Value, context?.Disposables);
    }

    public static (T value, CompositeDisposable disposables) Require(Control control)
    {
        var context = control.FindAncestorOfType<Context<T>>()
           ?? throw new InvalidOperationException($"ContextView<{typeof(T).Name}> not found in visual tree.");
        return (context.Value, context.Disposables);
    }
}
