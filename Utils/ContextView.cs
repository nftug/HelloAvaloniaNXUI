namespace HelloAvaloniaNXUI.Utils;

public class ContextView<T> : ContentControl where T : class
{
    public T Value { get; }

    public CompositeDisposable Disposables { get; private set; } = [];

    public ContextView(T value, Control content)
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

        Disposables?.Dispose();
        Disposables = [];
        (Value as IDisposable)?.Dispose();
    }

    public static ContextView<T>? Resolve(Control control)
        => control.FindAncestorOfType<ContextView<T>>();

    public static ContextView<T> Require(Control control)
        => control.FindAncestorOfType<ContextView<T>>()
           ?? throw new InvalidOperationException($"ContextView<{typeof(T).Name}> not found in visual tree.");
}
