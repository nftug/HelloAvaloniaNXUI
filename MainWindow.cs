using DialogHostAvalonia;
using HelloAvaloniaNXUI.Views;
using HelloAvaloniaNXUI.Views.Common;

namespace HelloAvaloniaNXUI;

public static class MainWindow
{
    public static Window Build() =>
        Window()
            .Title("Hello Avalonia NXUI").Width(1024).Height(680)
            .WindowStartupLocationCenterScreen()
            .Styles(new AppStyles())
            .Styles(new DialogHostStyles())
            // .Styles(new MaterialIconStyles(null))
            .Content(MainView.Build())
            .Init();

    private static Window Init(this Window window)
    {
        GetApplicationLifetime().ShutdownRequested += (_, _) => FileLockSingleInstanceGuard.ReleaseLock();

        window.Closing += (sender, e) =>
        {
            e.Cancel = true;
            (sender as Window)?.Hide();
        };

        window.Loaded += async (_, _) =>
        {
            if (!FileLockSingleInstanceGuard.TryAcquireLock())
            {
                await MessageBoxView.ShowAsync(
                    new("Application Already Running",
                        "Another instance of this application is already running.",
                        MessageBoxButton.Ok
                    ));
                GetApplicationLifetime().Shutdown();
            }
            else
            {
                var trayIcon = AppTrayIcon.Build();
                Avalonia.Controls.TrayIcon.SetIcons(Avalonia.Application.Current!, [trayIcon]);
            }
        };

        return window;
    }
}
