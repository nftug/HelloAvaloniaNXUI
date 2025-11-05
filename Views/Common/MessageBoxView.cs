using DialogHostAvalonia;

namespace HelloAvaloniaNXUI.Views.Common;

public enum MessageBoxButton
{
    Ok,
    OkCancel
}

public record MessageBoxProps(
    string Title,
    string Message,
    MessageBoxButton Buttons = MessageBoxButton.Ok,
    object? OkContent = null,
    object? CancelContent = null
);

public static class MessageBoxView
{
    private static StackPanel Build(MessageBoxProps props, ICommand closeCommand) =>
        StackPanel()
            .Margin(20)
            .Spacing(20)
            .MinWidth(300)
            .MaxWidth(400)
            .Children(
                TextBlock()
                    .Text(props.Title)
                    .FontSize(20)
                    .FontWeightBold()
                    .Margin(0, 0, 0, 10),
                TextBlock()
                    .Text(props.Message)
                    .FontSize(16)
                    .TextWrapping(TextWrapping.Wrap)
                    .Margin(0, 0, 0, 20),
                StackPanel()
                    .Orientation(Orientation.Horizontal)
                    .HorizontalAlignment(HorizontalAlignment.Right)
                    .Margin(0, 10, 0, 0)
                    .Spacing(10)
                    .Children(
                        props.Buttons == MessageBoxButton.OkCancel
                            ? [
                                Button()
                                    .Content(props.CancelContent ?? "Cancel")
                                    .OnClickHandler((_, _) => closeCommand.Execute(false))
                                    .Width(80),
                                Button()
                                    .Content(props.OkContent ?? "OK")
                                    .OnClickHandler((_, _) => closeCommand.Execute(true))
                                    .Width(80)
                            ]
                            : [
                                Button()
                                    .Content(props.OkContent ?? "OK")
                                    .OnClickHandler((_, _) => closeCommand.Execute(true))
                                    .Width(80)
                            ]
                    )
            );

    public static async Task<bool> ShowAsync(MessageBoxProps props, CancellationToken ct = default)
    {
        var dialogHost = GetControl<DialogHost>();
        var dialog = Build(props, dialogHost.CloseDialogCommand);
        var result = await DialogHost.Show(dialog);

        ct.ThrowIfCancellationRequested();

        return result is bool b && b;
    }
}
