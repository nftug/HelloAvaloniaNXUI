namespace HelloAvaloniaNXUI.Views.Common;

public static class AppTrayIcon
{
    public static TrayIcon Build()
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

        var trayIcon = new Avalonia.Controls.TrayIcon
        {
            ToolTipText = "Hello Avalonia NXUI",
            Icon = new WindowIcon(new Bitmap(NXUI.AssetLoader.Open(new Uri("avares://HelloAvaloniaNXUI/Assets/tray_icon.ico")))),
            Menu = nativeMenu
        };

        trayIcon.Clicked += (_, _) => showCommand.Execute(Unit.Default);

        return trayIcon;
    }
}
