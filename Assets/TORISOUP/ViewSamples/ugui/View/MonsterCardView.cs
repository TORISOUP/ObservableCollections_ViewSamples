using System;
using UnityEngine;
using UnityEngine.UI;

namespace ViewSamples.ugui.View
{
    /// <summary>
    /// Viewを管理するコンポーネント
    /// </summary>
    public class MonsterCardView : MonoBehaviour
    {
        [SerializeField] private Text _typeLabel;
        [SerializeField] private Text _nameLabel;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Button _removeButton;

        // 逃がすボタン
        public Button RemoveButton => _removeButton;

        /// <summary>
        /// Viewのセットアップ
        /// </summary>
        public void Initialize(Monster monster)
        {
            // 各種要素を設定
            _nameLabel.text = monster.Name;
            switch (monster.MonsterType)
            {
                case MonsterType.Fire:
                    _typeLabel.text = "ほのお";
                    _backgroundImage.color = new Color(1f, 0.7f, 0.7f);
                    break;
                case MonsterType.Water:
                    _typeLabel.text = "みず";
                    _backgroundImage.color = new Color(0.7f, 0.7f, 1.0f);
                    break;
                case MonsterType.Grass:
                    _typeLabel.text = "くさ";
                    _backgroundImage.color = new Color(0.7f, 1, 0.7f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}