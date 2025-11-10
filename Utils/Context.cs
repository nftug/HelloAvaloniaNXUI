namespace HelloAvaloniaNXUI.Utils;

public class Context<T> : ContentControl where T : class
{
    private T _value = default!;
    private CompositeDisposable _disposables = [];
    private readonly Func<CompositeDisposable, T>? _builder;

    private Context(T value, Control content)
    {
        _value = value;
        Content = content;
    }

    private Context(Func<CompositeDisposable, T> builder, Control content)
    {
        _builder = builder;
        Content = content;
    }

    public static Control Provide(Func<CompositeDisposable, T> builder, Control content)
        => new Context<T>(builder, content);

    public static Control Provide(T value, Control content)
        => new Context<T>(value, content);

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _disposables = [];

        if (_builder != null)
            _value = _builder.Invoke(_disposables);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _disposables.Dispose();
        _disposables = [];
        (_value as IDisposable)?.Dispose();
    }

    public static (T? value, CompositeDisposable? disposables) Resolve(Control control)
    {
        var context = control.FindAncestorOfType<Context<T>>();
        return (context?._value, context?._disposables);
    }

    public static (T value, CompositeDisposable disposables) Require(Control control)
    {
        var context = control.FindAncestorOfType<Context<T>>()
           ?? throw new InvalidOperationException($"Context<{typeof(T).Name}> not found in visual tree.");
        return (context._value, context._disposables);
    }
}
