using DialogHostAvalonia;

namespace HelloAvaloniaNXUI.Views;

public record ConfirmDialogProps(string Title, string Message);

public static class ConfirmDialogView
{
    public static Control Build(
        string title, string message, object? okContent, object? cancelContent, ICommand closeCommand) =>
        StackPanel()
            .Margin(20)
            .Spacing(20)
            .MinWidth(300)
            .MaxWidth(400)
            .Children(
                TextBlock()
                    .Text(title)
                    .FontSize(20)
                    .FontWeightBold()
                    .Margin(0, 0, 0, 10),
                TextBlock()
                    .Text(message)
                    .FontSize(16)
                    .TextWrapping(TextWrapping.Wrap)
                    .Margin(0, 0, 0, 20),
                StackPanel()
                    .Orientation(Orientation.Horizontal)
                    .HorizontalAlignment(HorizontalAlignment.Right)
                    .Margin(0, 10, 0, 0)
                    .Spacing(10)
                    .Children(
                        Button().Content(cancelContent ?? "Cancel")
                            .OnClickHandler((btn, _) => closeCommand.Execute(false))
                            .Width(80),
                        Button().Content(okContent ?? "OK")
                            .OnClickHandler((btn, _) => closeCommand.Execute(true))
                            .Width(80)
                    )
            );

    public static async Task<bool> ShowAsync(
        string title, string message, object? okContent = null, object? cancelContent = null)
    {
        var dialogHost = GetControl<DialogHost>();
        var dialog = Build(title, message, okContent, cancelContent, dialogHost.CloseDialogCommand);
        var result = await DialogHost.Show(dialog);
        return result is bool b && b;
    }
}
