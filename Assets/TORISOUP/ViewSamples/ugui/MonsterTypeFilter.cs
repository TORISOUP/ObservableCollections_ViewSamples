using ObservableCollections;
using ViewSamples.ugui.View;

namespace ViewSamples.ugui
{
    /// <summary>
    /// MonsterのTypeに応じて表示を切り替えるフィルター
    /// </summary>
    internal sealed class MonsterTypeFilter : ISynchronizedViewFilter<Monster, MonsterCardView>
    {
        private MonsterType MonsterType { get; }

        public MonsterTypeFilter(MonsterType monsterType)
        {
            MonsterType = monsterType;
        }

        public bool IsMatch(Monster value, MonsterCardView view)
        {
            // タイプが一致するか
            return value.MonsterType == MonsterType;
        }

        public void WhenTrue(Monster value, MonsterCardView view)
        {
            view.gameObject.SetActive(true);
        }

        public void WhenFalse(Monster value, MonsterCardView view)
        {
            view.gameObject.SetActive(false);
        }

        public void OnCollectionChanged(ChangedKind changedKind,
            Monster value,
            MonsterCardView view,
            in NotifyCollectionChangedEventArgs<Monster> eventArgs)
        {
            // do nothing
        }
    }
}