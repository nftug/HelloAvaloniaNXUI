namespace HelloAvaloniaNXUI.Utils;

public static class ApplicationUtils
{
    public static IClassicDesktopStyleApplicationLifetime GetApplicationLifetime()
    {
        return Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime
            ?? throw new InvalidOperationException("Desktop application lifetime is not available.");
    }

    public static Window GetMainWindow()
    {
        return GetApplicationLifetime().MainWindow
            ?? throw new InvalidOperationException("Main window is not available.");
    }

    public static TControl GetControl<TControl>()
        where TControl : Control
    {
        var window = GetMainWindow();
        return window.FindDescendantOfType<TControl>()
            ?? throw new InvalidOperationException($"Control of type {typeof(TControl).Name} not found.");
    }

    public static async Task InvokeAsync(CompositeDisposable disposables, Func<CancellationToken, Task> work)
    {
        // Create a linked cancellation token source
        var cts = new CancellationTokenSource();
        disposables.Add(Disposable.Create(cts.Cancel));

        try
        {
            await work(cts.Token);
        }
        catch (OperationCanceledException)
        {
        }
        catch (ObjectDisposedException)
        {
        }
    }

    public static Control WithReactive<TControl>(Func<CompositeDisposable, ContentControl, TControl> onAttached)
        where TControl : Control
    {
        CompositeDisposable? disposables = null;
        var container = new ContentControl();

        container.AttachedToVisualTree += (_, _) =>
        {
            disposables = [];
            container.Content = onAttached(disposables, container);
        };

        container.DetachedFromVisualTree += (_, _) =>
        {
            disposables?.Dispose();
            disposables = null;
        };

        return container;
    }
}
