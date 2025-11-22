using HelloAvaloniaNXUI.Views.Common;
using Material.Icons;

namespace HelloAvaloniaNXUI.Views.CounterListPage;

public static class CounterListPageView
{
    private static StackPanel BuildCounterItem(ReactiveProperty<int> count) =>
        StackPanel()
            .OrientationHorizontal()
            .Spacing(12)
            .HorizontalAlignmentCenter()
            .Margin(4)
            .Children(
                Label()
                    .MinWidth(40)
                    .FontSize(20)
                    .FontWeightBold()
                    .Content(count.AsSystemObservable().ToBinding()),
                Button()
                    .Content("+")
                    .OnClickHandler((_, e) => count.Value++)
                    .Width(40),
                Button()
                    .Content("-")
                    .OnClickHandler((_, e) => count.Value--)
                    .Width(40)
            );

    public static Control Build() =>
        WithLifecycle((disposables, _) =>
        {
            var counters = new ObservableCollection<ReactiveProperty<int>>(
                Enumerable.Range(0, 5).Select(i => new ReactiveProperty<int>(i).AddTo(disposables))
            );

            var countersLength = counters.ObservePropertyChanged(c => c.Count);

            var countersSum = counters.MapFromCollectionChanged(items => items.Sum());

            void handleClickAdd()
            {
                var nextValue = counters.Count > 0 ? counters[^1].Value + 1 : 0;
                counters.Add(new ReactiveProperty<int>(nextValue).AddTo(disposables));
            }

            void handleClickRemove()
            {
                if (counters.Count == 0) return;
                var last = counters[^1];
                counters.RemoveAt(counters.Count - 1);
                last.Dispose();
            }

            return DockPanel()
                .Margin(10)
                .Children(
                    StackPanel()
                        .DockTop()
                        .Spacing(15)
                        .Margin(0, 0, 0, 10)
                        .Children(
                            StackPanel()
                                .OrientationHorizontal()
                                .Spacing(12)
                                .HorizontalAlignmentCenter()
                                .Children(
                                    Button()
                                        .Content("Add Counter")
                                        .OnClickHandler((_, _) => handleClickAdd())
                                        .Width(100)
                                        .Height(40),
                                    Button()
                                        .Content("Remove Last")
                                        .OnClickHandler((_, _) => handleClickRemove())
                                        .IsEnabled(countersLength.Select(x => x > 0).AsSystemObservable())
                                        .Width(100)
                                        .Height(40)
                                ),
                            TextBlock()
                                .Text(countersSum.Select(sum => $"Counters Sum: {sum}").AsSystemObservable())
                                .FontSize(24)
                                .FontWeightBold()
                                .HorizontalAlignmentCenter()
                        ),
                    ScrollViewer()
                        .DockBottom()
                        .Content(
                            ItemsControl()
                                .ItemsSource(counters)
                                .ItemTemplateObservable<int>(BuildCounterItem)
                        )
                        .Margin(10)
                        .VerticalScrollBarVisibilityAuto()
                        .HorizontalScrollBarVisibilityDisabled()
                );
        });

    public static PageItem BuildPageItem() =>
        new(MaterialIconKind.Counter, "Counter List", Build());
}
