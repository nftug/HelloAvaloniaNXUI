using HelloAvaloniaNXUI.Views.Common;
using Material.Icons;

namespace HelloAvaloniaNXUI.Views.CounterListPage;

public static class CounterListPage
{
    public static Control Build() =>
        WithReactive((disposables) =>
        {
            var counters = new ObservableCollection<ReactiveProperty<int>>(
                Enumerable.Range(0, 5).Select(i => new ReactiveProperty<int>(i).AddTo(disposables))
            );

            var countersLength = counters.ObservePropertyChanged(c => c.Count);

            var countersSum = counters
                .ObservePropertyChanged(c => c.Count)
                .Prepend(0)
                .SelectMany(_ =>
                    counters.Count == 0
                        ? R3.Observable.Return(0)
                        : R3.Observable.CombineLatest(counters).Select(v => v.Sum()));

            var addButton = Button()
                .Content("Add Counter")
                .OnClickHandler((_, _) =>
                {
                    var nextValue = counters.Count > 0 ? counters[^1].Value + 1 : 0;
                    counters.Add(new ReactiveProperty<int>(nextValue).AddTo(disposables));
                })
                .Width(100)
                .Height(40);

            var removeButton = Button()
                .Content("Remove Last")
                .OnClickHandler((_, _) =>
                {
                    if (counters.Count > 0)
                        counters.RemoveAt(counters.Count - 1);
                })
                .IsEnabled(countersLength.Select(x => x > 0).AsSystemObservable())
                .Width(100)
                .Height(40);

            var listView = ItemsControl()
                .ItemsSource(counters)
                .ItemTemplate(new FuncDataTemplate<ReactiveProperty<int>>((count, _) =>
                    StackPanel()
                        .OrientationHorizontal()
                        .Spacing(12)
                        .HorizontalAlignmentCenter()
                        .Margin(4)
                        .Children(
                            Label()
                                .MinWidth(40)
                                .FontSize(20)
                                .FontWeight(FontWeight.Bold)
                                .Content(count.AsSystemObservable().ToBinding()),
                            Button()
                                .Content("+")
                                .OnClickHandler((_, e) => count.Value++)
                                .Width(40),
                            Button()
                                .Content("-")
                                .OnClickHandler((_, e) => count.Value--)
                                .Width(40)
                        ), true)
                );

            return DockPanel()
                .Margin(10)
                .Children(
                    StackPanel()
                        .Dock(Dock.Top)
                        .Spacing(15)
                        .Margin(0, 0, 0, 10)
                        .Children(
                            StackPanel()
                                .OrientationHorizontal()
                                .Spacing(12)
                                .HorizontalAlignmentCenter()
                                .Children(addButton, removeButton),
                            TextBlock()
                                .Text(countersSum.Select(sum => $"Counters Sum: {sum}").AsSystemObservable())
                                .FontSize(24)
                                .FontWeight(FontWeight.Bold)
                                .HorizontalAlignment(HorizontalAlignment.Center)
                        ),
                    ScrollViewer()
                        .Dock(Dock.Bottom)
                        .Content(listView)
                        .Margin(10)
                        .VerticalScrollBarVisibility(ScrollBarVisibility.Auto)
                        .HorizontalScrollBarVisibility(ScrollBarVisibility.Disabled)
                );
        });

    public static PageItem BuildPageItem() =>
        new(MaterialIconKind.Counter, "Counter List", Build());
}
