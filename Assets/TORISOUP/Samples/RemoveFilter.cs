using ObservableCollections;
using UnityEngine;

namespace Samples
{
    public class GameObjectRemoveFilter<T> : ISynchronizedViewFilter<T, GameObject>
    {
        public bool IsMatch(T value, GameObject view)
        {
            return true;
        }

        public void WhenTrue(T value, GameObject view)
        {
        }

        public void WhenFalse(T value, GameObject view)
        {
        }

        public void OnCollectionChanged(ChangedKind changedKind,
            T value,
            GameObject view,
            in NotifyCollectionChangedEventArgs<T> eventArgs)
        {
            Debug.Log($"{changedKind}: {view}");

            if (changedKind == ChangedKind.Remove && view != null)
            {
                Object.Destroy(view);
            }
        }
    }

    public class ComponentRemoveFilter<T> : ISynchronizedViewFilter<T, UnityEngine.Component>
    {
        public bool IsMatch(T value, Component view)
        {
            return true;
        }

        public void WhenTrue(T value, Component view)
        {
        }

        public void WhenFalse(T value, Component view)
        {
        }

        public void OnCollectionChanged(ChangedKind changedKind,
            T value,
            Component view,
            in NotifyCollectionChangedEventArgs<T> eventArgs)
        {
            if (changedKind == ChangedKind.Remove && view != null)
            {
                Object.Destroy(view);
            }
        }
    }
}