using Avalonia.Controls.Notifications;

namespace HelloAvaloniaNXUI.Contexts;

public class ViewContext
{
    public WindowNotificationManager? NotificationManager { get; private set; }

    public void Inject(WindowNotificationManager notificationManager)
        => NotificationManager = notificationManager;
}
