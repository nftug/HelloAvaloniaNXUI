using System.ComponentModel;

namespace HelloAvaloniaNXUI.Views.Common;

public static class TrayIconExtensions
{
    public static TrayIcon BuildTrayIcon()
    {
        var showCommand = new ReactiveCommand();
        showCommand.Subscribe(_ =>
        {
            var mainWindow = GetMainWindow();
            if (mainWindow == null) return;

            mainWindow.Show();
            mainWindow.WindowState = WindowState.Normal;
            mainWindow.Activate();
        });

        var exitCommand = new ReactiveCommand();
        exitCommand.Subscribe(async _ =>
        {
            showCommand.Execute(Unit.Default);
            var confirmed = await MessageBoxView.ShowAsync(
                new("Exit Application",
                    "Are you sure you want to exit the application?",
                    MessageBoxButton.OkCancel)
            );
            if (!confirmed) return;

            GetApplicationLifetime().Shutdown();
        });

        var nativeMenu = new NativeMenu
        {
            NativeMenuItem().Header("Show").Command(showCommand),
            NativeMenuItem().Header("Exit").Command(exitCommand)
        };

        return new TrayIcon
        {
            ToolTipText = "Hello Avalonia NXUI",
            Icon = new WindowIcon(new Bitmap(NXUI.AssetLoader.Open(new Uri("avares://HelloAvaloniaNXUI/Assets/tray_icon.ico")))),
            Menu = nativeMenu
        };
    }

    public static Window AppTrayIcon(this Window window)
    {
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
                var trayIcon = BuildTrayIcon();
                Avalonia.Controls.TrayIcon.SetIcons(Avalonia.Application.Current!, [trayIcon]);
            }
        };

        static void HandleClosingRequested(object? sender, CancelEventArgs e)
        {
            if (sender is not Window win) return;
            e.Cancel = true;
            win.Hide();
        }

        window.Closing += HandleClosingRequested;
        GetApplicationLifetime().ShutdownRequested += (_, _) => FileLockSingleInstanceGuard.ReleaseLock();

        return window;
    }
}
