using System.ComponentModel;

namespace HelloAvaloniaNXUI.Views;

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
            var confirmed = await ConfirmDialogView.ShowAsync(
                "Exit Application",
                "Are you sure you want to exit the application?",
                "Exit",
                "Cancel"
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
        var trayIcon = BuildTrayIcon();
        Avalonia.Controls.TrayIcon.SetIcons(Avalonia.Application.Current!, [trayIcon]);

        static void HandleClosingRequested(object? sender, CancelEventArgs e)
        {
            if (sender is not Window win) return;
            e.Cancel = true;
            win.Hide();
        }

        GetApplicationLifetime().ShutdownRequested += HandleClosingRequested;
        window.Closing += HandleClosingRequested;

        return window;
    }
}
