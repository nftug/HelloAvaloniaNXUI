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
    private static StackPanel Build(MessageBoxProps props, ICommand closeCommand)
    {
        Control CreateButton(object content, Action onClick) =>
            Button()
                .Content(content)
                .OnClickHandler((_, _) => onClick())
                .Width(80);

        var okButton = CreateButton(props.OkContent ?? "OK", () => closeCommand.Execute(true));
        var cancelButton = CreateButton(props.CancelContent ?? "Cancel", () => closeCommand.Execute(false));

        return StackPanel()
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
                    .TextWrappingWrap()
                    .Margin(0, 0, 0, 20),
                StackPanel()
                    .OrientationHorizontal()
                    .HorizontalAlignmentRight()
                    .Margin(0, 10, 0, 0)
                    .Spacing(10)
                    .Children(
                        props.Buttons == MessageBoxButton.OkCancel
                            ? [cancelButton, okButton]
                            : [okButton]
                    )
            );
    }

    public static async Task<bool> ShowAsync(MessageBoxProps props, CancellationToken ct = default)
    {
        var dialogHost = GetControlFromWindow<DialogHost>();
        var dialog = Build(props, dialogHost.CloseDialogCommand);
        var result = await DialogHost.Show(dialog);

        ct.ThrowIfCancellationRequested();

        return result is bool b && b;
    }
}
