using DialogHostAvalonia;

namespace HelloAvaloniaNXUI.Views;

public record ConfirmDialogProps(string Title, string Message);

public static class ConfirmDialogView
{
    public static Control Build(ConfirmDialogProps props, ICommand closeCommand) =>
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
                        Button().Content("Cancel")
                            .OnClickHandler((btn, _) => closeCommand.Execute(false))
                            .Width(80),
                        Button().Content("OK")
                            .OnClickHandler((btn, _) => closeCommand.Execute(true))
                            .Width(80)
                    )
            );

    public static async Task<bool> ShowAsync(string title, string message)
    {
        var dialogHost = GetControl<DialogHost>();
        var dialog = Build(new(title, message), dialogHost.CloseDialogCommand);
        var result = await DialogHost.Show(dialog);
        return result is bool b && b;
    }
}
