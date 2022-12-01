using ObservableCollections;
using UnityEngine;

namespace ViewSamples
{
    /// <summary>
    /// モンスターを格納するオブジェクト
    /// </summary>
    public class MonsterBackpack : MonoBehaviour
    {
        // View構築用にpublicにしておく
        public IObservableCollection<Monster> ObservableCollection => _hashSet;

        // ObservableなHashSet
        // HashSetは要素の重複を許さないコレクション
        private readonly ObservableHashSet<Monster> _hashSet = new();

        // BackPackの最大容量
        private readonly int _maxCapacity = 6;

        /// <summary>
        /// モンスターを追加する
        /// </summary>
        public bool TryAddMonster(MonsterType monsterType, string name)
        {
            var monster = new Monster(monsterType, name);

            // 要素を超える場合は追加しない
            if (_hashSet.Count >= _maxCapacity) return false;
            return _hashSet.Add(monster);
        }

        /// <summary>
        /// モンスターを除去する
        /// </summary>
        public void RemoveMonster(Monster monster)
        {
            _hashSet.Remove(monster);
        }

        /// <summary>
        /// すべてのモンスターを除去する
        /// </summary>
        public void RemoveAllMonster()
        {
            _hashSet.Clear();
        }
    }
}