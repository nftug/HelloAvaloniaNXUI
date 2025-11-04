using Material.Icons;
using Material.Icons.Avalonia;

namespace HelloAvaloniaNXUI.Views;

public sealed record PageItem(MaterialIconKind Icon, string Title, Control View);

public sealed record NavigationViewProps(IReadOnlyList<PageItem> Pages, Action<int>? OnSelectedIndexChanged = null);

public sealed record DrawerViewProps(
    ReactiveProperty<bool> IsOpened,
    ReactiveProperty<int> SelectedIndex,
    IReadOnlyList<PageItem> Pages
);

public static class NavigationView
{
    private static DockPanel BuildDrawer(DrawerViewProps props) =>
        DockPanel()
            .LastChildFill(true)
            .Margin(12)
            .Children(
                StackPanel()
                    .Children([.. props.Pages
                        .Select((page, index) =>
                            ToggleButton()
                                .Content(
                                    StackPanel()
                                        .Orientation(Orientation.Horizontal)
                                        .Spacing(10)
                                        .Children(
                                            new MaterialIcon() { Kind = page.Icon },
                                            TextBlock()
                                                .Text(page.Title)
                                                .FontSize(16)
                                                .VerticalAlignment(VerticalAlignment.Center)
                                        )
                                )
                                .IsChecked(
                                    props.SelectedIndex
                                        .Select(selectedIndex => (bool?)(selectedIndex == index))
                                        .AsSystemObservable())
                                .OnClickHandler((_, _) =>
                                {
                                    props.SelectedIndex.Value = index;
                                    props.IsOpened.Value = false;
                                })
                                .Margin(0, 5, 0, 5)
                                .Height(40)
                                .HorizontalAlignment(HorizontalAlignment.Stretch)
                                .HorizontalContentAlignment(HorizontalAlignment.Left)
                                .Background(Brushes.Transparent)
                                .BorderBrush(Brushes.Transparent)
                        )]));

    public static Control Build(NavigationViewProps props) => WithReactive(disposables =>
    {
        var drawerIsOpened = new ReactiveProperty<bool>(false).AddTo(disposables);
        var selectedIndex = new ReactiveProperty<int>(0).AddTo(disposables);

        selectedIndex
            .Subscribe(index => props.OnSelectedIndexChanged?.Invoke(index))
            .AddTo(disposables);

        var pageTitle = selectedIndex
            .Select(index => props.Pages[index].Title)
            .ToReadOnly(disposables, string.Empty);

        var header = StackPanel()
            .Dock(Dock.Top)
            .Orientation(Orientation.Horizontal)
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
                    .VerticalAlignment(VerticalAlignment.Center)
                    .Margin(20, 0, 0, 0)
            );

        return SplitView()
            .DisplayMode(SplitViewDisplayMode.Overlay)
            .OpenPaneLength(250)
            .IsPaneOpen(drawerIsOpened.AsSystemObservable())
            .OnPaneClosedHandler((_, _) => drawerIsOpened.Value = false)
            .Pane(BuildDrawer(new(drawerIsOpened, selectedIndex, props.Pages)))
            .Content(
                DockPanel()
                    .LastChildFill(true)
                    .Children(
                        header,
                        ContentControl()
                            .Content(
                                selectedIndex
                                    .Select(index => props.Pages[index].View)
                                    .AsSystemObservable()
                            )
                    )
            );
    });
}