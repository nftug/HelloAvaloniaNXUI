using Avalonia.Controls.Notifications;

namespace HelloAvaloniaNXUI.Utils;

public static class NotificationView
{
    public static void Show(string title, string message, NotificationType type)
        => GetControl<WindowNotificationManager>().Show(new Notification(title, message, type));
}
