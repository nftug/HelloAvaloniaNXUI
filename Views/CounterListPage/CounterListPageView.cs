using HelloAvaloniaNXUI.Views.Common;
using Material.Icons;

namespace HelloAvaloniaNXUI.Views.CounterListPage;

public static class CounterListPageView
{
    public record CounterState(Guid Id, int Value);

    private static Control BuildCounterItem(ObservableList<CounterState> items, CounterState item)
    {
        void UpdateItemValue(int value)
        {
            var target = items.FirstOrDefault(i => i.Id == item.Id);
            if (target == null) return;
            var index = items.IndexOf(target);
            items[index] = target with { Value = value };
        }

        return StackPanel()
            .OrientationHorizontal()
            .Spacing(12)
            .HorizontalAlignmentCenter()
            .Margin(4)
            .Children(
                Label()
                    .MinWidth(40)
                    .FontSize(20)
                    .FontWeightBold()
                    .Content(item.Value),
                Button()
                    .Content("+")
                    .OnClickHandler((_, e) => UpdateItemValue(item.Value + 1))
                    .Width(40),
                Button()
                    .Content("-")
                    .OnClickHandler((_, e) => UpdateItemValue(item.Value - 1))
                    .Width(40)
            );
    }

    public static Control Build() =>
        WithLifecycle((disposables, _) =>
        {
            var counters = new ObservableList<CounterState>(
                Enumerable.Range(0, 5).Select(i => new CounterState(Guid.NewGuid(), i))
            );

            var countersLength = counters.ObserveCountChanged();

            var countersSum = counters.ObserveChanged()
                .Select(_ => Unit.Default)
                .Prepend(Unit.Default)
                .Select(_ => counters.Sum(c => c.Value));

            void HandleClickAdd()
            {
                var nextValue = counters.Count > 0 ? counters[^1].Value + 1 : 0;
                counters.Add(new CounterState(Guid.NewGuid(), nextValue));
            }

            void HandleClickRemove()
            {
                if (counters.Count == 0) return;
                counters.RemoveAt(counters.Count - 1);
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
                                        .OnClickHandler((_, _) => HandleClickAdd())
                                        .Width(100)
                                        .Height(40),
                                    Button()
                                        .Content("Remove Last")
                                        .OnClickHandler((_, _) => HandleClickRemove())
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
                    ItemsControl()
                        .ItemsSource(counters.ToNotifyCollectionChangedSlim())
                        .ItemTemplateFunc<CounterState>(v => BuildCounterItem(counters, v))
                        .TemplateFunc(
                            ScrollViewer()
                                .Content(ItemsPresenter())
                                .VerticalScrollBarVisibilityAuto()
                                .HorizontalScrollBarVisibilityDisabled()
                        )
                        .ItemsPanelFunc(VirtualizingStackPanel())
                        .DockBottom()
                        .Margin(10)
                );
        });

    public static PageItem BuildPageItem() =>
        new(MaterialIconKind.Counter, "Counter List", Build());
}
