namespace HelloAvaloniaNXUI.Utils;

using System.IO;
using System.Reflection;

public static class FileLockSingleInstanceGuard
{
    private static readonly string appName = Assembly.GetEntryAssembly()?.GetName().Name ?? "HelloAvaloniaNXUI";
    private static readonly string _lockPath = GetLockFilePath();
    private static FileStream? _stream;

    private static string GetLockFilePath()
    {
        string appDir;

        if (OperatingSystem.IsLinux())
        {
            var dataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
            var baseDir = string.IsNullOrEmpty(dataHome)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local", "share")
                : dataHome;
            appDir = Path.Combine(baseDir, appName);
        }
        else
        {
            appDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                appName
            );
        }

        Directory.CreateDirectory(appDir);
        return Path.Combine(appDir, "app.lock");
    }

    public static bool TryAcquireLock()
    {
        try
        {
            _stream = new(_lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            return true;
        }
        catch (IOException)
        {
            return false;
        }
    }

    public static void ReleaseLock()
    {
        try { _stream?.Dispose(); } catch { }
        _stream = null;
        try { if (File.Exists(_lockPath)) File.Delete(_lockPath); } catch { }
    }
}
