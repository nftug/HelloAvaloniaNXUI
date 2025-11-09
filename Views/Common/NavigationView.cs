using Material.Icons;
using Material.Icons.Avalonia;

namespace HelloAvaloniaNXUI.Views.Common;

public sealed record PageItem(MaterialIconKind Icon, string Title, Control View);

public sealed record NavigationViewProps(
    IReadOnlyDictionary<string, PageItem> Pages, string InitialPage, Action<string>? OnCurrentPageChanged = null);

public sealed record DrawerViewProps(
    ReactiveProperty<bool> IsOpened,
    ReactiveProperty<string> SelectedKey,
    IReadOnlyDictionary<string, PageItem> Pages
);

public static class NavigationView
{
    private static ContentControl BuildDrawer(DrawerViewProps props) =>
        WithReactive((disposables) =>
            StackPanel()
                .Margin(10)
                .Children(
                    [.. props.Pages.Select(page => {
                        var isSelected = props.SelectedKey
                            .Select(selected => (bool?)(selected == page.Key))
                            .ToReactiveValue(disposables);

                        return ToggleButton()
                            .Content(
                                StackPanel()
                                    .OrientationHorizontal()
                                    .Spacing(10)
                                    .Children(
                                        new MaterialIcon() { Kind = page.Value.Icon },
                                        TextBlock()
                                            .Text(page.Value.Title)
                                            .FontSize(16)
                                            .VerticalAlignmentCenter()
                                    )
                            )
                            .IsChecked(isSelected.AsSystemObservable())
                            .OnIsCheckedChangedHandler((ctl, _) => ctl.IsChecked = isSelected.CurrentValue)
                            .OnClickHandler((_, _) =>
                            {
                                props.SelectedKey.Value = page.Key;
                                props.IsOpened.Value = false;
                            })
                            .Margin(0, 5, 0, 5)
                            .Height(40)
                            .HorizontalAlignmentStretch()
                            .HorizontalContentAlignmentLeft()
                            .Background(Brushes.Transparent)
                            .BorderBrush(Brushes.Transparent);
                    })]
                )
            );

    public static Control Build(NavigationViewProps props) =>
        WithReactive(disposables =>
        {
            var drawerIsOpened = new ReactiveProperty<bool>(false).AddTo(disposables);
            var selectedKey = new ReactiveProperty<string>(props.InitialPage).AddTo(disposables);

            selectedKey
                .Subscribe(key => props.OnCurrentPageChanged?.Invoke(key))
                .AddTo(disposables);

            var pageTitle = selectedKey
                .Select(key => props.Pages.TryGetValue(key, out var page) ? page.Title : "Unknown")
                .ToReactiveValue(disposables, string.Empty);

            var header = StackPanel()
                .Dock(Dock.Top)
                .OrientationHorizontal()
                .Height(50)
                .Margin(5)
                .Children(
                    Button()
                        .Content(new MaterialIcon() { Kind = MaterialIconKind.Menu })
                        .Width(50)
                        .Height(50)
                        .FontSize(18)
                        .Background(Brushes.Transparent)
                        .BorderBrush(Brushes.Transparent)
                        .OnClickHandler((_, _) => drawerIsOpened.Value = true),
                    TextBlock()
                        .Text(pageTitle.AsSystemObservable())
                        .FontSize(21)
                        .VerticalAlignmentCenter()
                        .Margin(20, 0, 0, 0)
                );

            return SplitView()
                .DisplayModeOverlay()
                .UseLightDismissOverlayMode(true)
                .OpenPaneLength(250)
                .IsPaneOpen(drawerIsOpened.AsSystemObservable())
                .OnPaneClosedHandler((_, _) => drawerIsOpened.Value = false)
                .Pane(BuildDrawer(new(drawerIsOpened, selectedKey, props.Pages)))
                .Content(
                    DockPanel()
                        .LastChildFill(true)
                        .Children(
                            header,
                            ContentControl()
                                .Content(
                                    selectedKey
                                        .Select(key => props.Pages.TryGetValue(key, out var page) ? page.View : null)
                                        .AsSystemObservable()
                                )
                        )
                );
        });
}