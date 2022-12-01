using ObservableCollections;
using UnityEngine;
using ViewSamples.ugui.View;

namespace ViewSamples.ugui
{
    // Remove時に要素を削除するフィルター
    internal sealed class RemoveFilter : ISynchronizedViewFilter<Monster, MonsterCardView>
    {
        public bool IsMatch(Monster value, MonsterCardView view)
        {
            return true;
        }

        public void WhenTrue(Monster value, MonsterCardView view)
        {
        }

        public void WhenFalse(Monster value, MonsterCardView view)
        {
        }

        public void OnCollectionChanged(ChangedKind changedKind,
            Monster value,
            MonsterCardView view,
            in NotifyCollectionChangedEventArgs<Monster> eventArgs)
        {
            if (changedKind == ChangedKind.Remove)
            {
                if (view != null) Object.Destroy(view.gameObject);
            }
        }
    }
}