using Avalonia.Controls.Notifications;

namespace HelloAvaloniaNXUI.Utils;

public static class ApplicationUtils
{
    public static Window GetMainWindow()
    {
        return Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow!
            : throw new InvalidOperationException("Main window is not available.");
    }

    public static TControl GetControl<TControl>()
        where TControl : Control
    {
        var window = GetMainWindow();
        return window.GetVisualDescendants().OfType<TControl>().FirstOrDefault()
            ?? throw new InvalidOperationException($"Control of type {typeof(TControl).Name} not found.");
    }

    public static void ShowNotification(Notification notification)
    {
        var notificationManager = GetControl<WindowNotificationManager>();
        notificationManager.Show(notification);
    }
}
