using System;
using System.Collections.Specialized;
using ObservableCollections;
using UniRx;

namespace Samples
{
    public static class ObservableCollectionsExtension
    {
        public static IObservable<NotifyCollectionChangedEventArgs> AsObservable<T>(this IObservableCollection<T> collection)
        {
            return Observable
                .FromEvent<NotifyCollectionChangedEventHandler<T>,
                    NotifyCollectionChangedEventArgs>(
                    h => (in NotifyCollectionChangedEventArgs<T> args) => h(args.ToStandardEventArgs()),
                    h => collection.CollectionChanged += h,
                    h => collection.CollectionChanged -= h);
        }
    }
}