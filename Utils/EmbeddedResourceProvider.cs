using System.Reflection;

namespace HelloAvaloniaNXUI.Utils;

public static class EmbeddedResourceProvider
{
    private static string DetectResourceNamespace(Assembly assembly, string rootPath)
    {
        var segment = rootPath.TrimStart('/').Replace('/', '.');

        var matched =
            assembly.GetManifestResourceNames()
                .FirstOrDefault(r => r.Contains(segment, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"Resource root path '{rootPath}' not found in assembly '{assembly.FullName}'.");

        var index = matched.IndexOf(segment, StringComparison.OrdinalIgnoreCase);
        return matched[..index].TrimEnd('.');
    }

    public static Stream GetFileStream(string filePath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        filePath = filePath.Replace('\\', '/').TrimStart('/');
        var rootPath = filePath.Split('/')[0];

        var resourceNamespace = DetectResourceNamespace(assembly, rootPath);
        var fullResourceName = $"{resourceNamespace}.{filePath.Replace('/', '.')}";

        return assembly.GetManifestResourceStream(fullResourceName)
            ?? throw new FileNotFoundException($"Embedded resource '{fullResourceName}' not found in assembly '{assembly.FullName}'.");
    }
}
