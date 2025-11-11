using HelloAvaloniaNXUI;

AppBuilder.Configure<Application>()
  .UsePlatformDetect()
  .UseFluentTheme()
  .UseR3()
  .WithApplicationName("HelloAvaloniaNXUI")
  .StartWithClassicDesktopLifetime(MainWindow.Build, args);
