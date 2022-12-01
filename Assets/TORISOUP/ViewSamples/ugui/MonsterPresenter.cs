using ObservableCollections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using ViewSamples.ugui.View;

namespace ViewSamples.ugui
{
    /// <summary>
    /// MonsterBackpackの要素をリストへ反映する
    /// </summary>
    public sealed class MonsterPresenter : MonoBehaviour
    {
        [SerializeField] private MonsterBackpack _monsterBackpack;
        [SerializeField] private MonsterCardView _cardPrefab;
        [SerializeField] private Transform _viewRoot;

        [SerializeField] private Dropdown _filterDropdown;

        private ISynchronizedView<Monster, MonsterCardView> _synchronizedView;

        private void Start()
        {
            // コレクションとViewの連動を定義
            _synchronizedView = _monsterBackpack.ObservableCollection.CreateView(m =>
            {
                // Monsterが追加されたらPrefabより新しいViewを追加
                var view = Instantiate(_cardPrefab, _viewRoot.transform);
                view.Initialize(m);

                // Viewのボタンを購読し、逃がすボタンが押されたら要素から消すように定義
                view.RemoveButton.OnClickAsObservable()
                    .Take(1)
                    .Subscribe(_ => _monsterBackpack.RemoveMonster(m))
                    .AddTo(view);

                return view;
            });

            // Dropdownが操作されたらフィルターを設定する
            _filterDropdown.OnValueChangedAsObservable()
                .Subscribe(x =>
                {
                    switch (x)
                    {
                        case 0:
                            // フィルタを一旦解除し、描画設定を元に戻す
                            _synchronizedView.ResetFilter((_, v) =>
                                v.gameObject.SetActive(true));
                            // 改めて削除フィルタのみ再設定
                            _synchronizedView.AttachFilter(new RemoveFilter());
                            break;
                        case 1: // Fire
                            _synchronizedView.AttachFilter(
                                new FilterMux<Monster, MonsterCardView>(
                                    new MonsterTypeFilter(MonsterType.Fire),
                                    new RemoveFilter()));
                            break;
                        case 2: // Water
                            _synchronizedView.AttachFilter(
                                new FilterMux<Monster, MonsterCardView>(
                                    new MonsterTypeFilter(MonsterType.Water),
                                    new RemoveFilter()));
                            break;
                        case 3: // Grass
                            _synchronizedView.AttachFilter(
                                new FilterMux<Monster, MonsterCardView>(
                                    new MonsterTypeFilter(MonsterType.Grass),
                                    new RemoveFilter()));
                            break;
                    }
                })
                .AddTo(this);

            // 初期設定として削除フィルターを追加
            _synchronizedView.AttachFilter(new RemoveFilter());
        }
    }
}