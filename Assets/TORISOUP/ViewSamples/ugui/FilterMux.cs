using System.Linq;
using ObservableCollections;

namespace ViewSamples.ugui
{
    /// <summary>
    /// 複数のFilterを合成するもの
    /// </summary>
    public sealed class FilterMux<T, TView> : ISynchronizedViewFilter<T, TView>
    {
        private readonly ISynchronizedViewFilter<T, TView>[] _filters;

        public FilterMux(params ISynchronizedViewFilter<T, TView>[] filters)
        {
            _filters = filters;
        }

        public bool IsMatch(T value, TView view)
        {
            return _filters.All(x => x.IsMatch(value, view));
        }

        public void WhenTrue(T value, TView view)
        {
            foreach (var filter in _filters)
            {
                filter.WhenTrue(value, view);
            }
        }

        public void WhenFalse(T value, TView view)
        {
            foreach (var filter in _filters)
            {
                filter.WhenFalse(value, view);
            }
        }

        public void OnCollectionChanged(ChangedKind changedKind,
            T value,
            TView view,
            in NotifyCollectionChangedEventArgs<T> eventArgs)
        {
            foreach (var filter in _filters)
            {
                filter.OnCollectionChanged(changedKind, value, view, eventArgs);
            }
        }
    }
}