namespace HelloAvaloniaNXUI.Utils;

public class ContextProvider<T> : ContentControl where T : class
{
    private T _value = default!;
    private CompositeDisposable _disposables = [];
    private readonly Func<CompositeDisposable, T>? _builder;

    private ContextProvider(T value, Control content, string? name = null)
    {
        _value = value;
        Content = content;
        Name = name;
    }

    private ContextProvider(Func<CompositeDisposable, T> builder, Control content, string? name = null)
    {
        _builder = builder;
        Content = content;
        Name = name;
    }

    public static Control Provide(Func<CompositeDisposable, T> builder, Control content, string? name = null)
        => new ContextProvider<T>(builder, content, name);

    public static Control Provide(T value, Control content, string? name = null)
        => new ContextProvider<T>(value, content, name);

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

    public static (T? value, CompositeDisposable? disposables) Resolve(Control control, string? name = null)
    {
        var context = FindContextFromAncestors(control, name);
        return (context?._value, context?._disposables);
    }

    public static (T value, CompositeDisposable disposables) Require(Control control, string? name = null)
    {
        var context = FindContextFromAncestors(control, name)
           ?? throw new InvalidOperationException($"Context<{typeof(T).Name}> not found in visual tree.");
        return (context._value, context._disposables);
    }

    private static ContextProvider<T>? FindContextFromAncestors(Control control, string? name)
    {
        return control.GetVisualAncestors()
           .OfType<ContextProvider<T>>()
           .FirstOrDefault(c => name == null || c.Name == name);
    }
}
