using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ViewSamples.ugui
{
    /// <summary>
    /// 各種UIのボタン操作をModelに反映する
    /// </summary>
    public sealed class ControllerMenuPresenter : MonoBehaviour
    {
        [SerializeField] private MonsterBackpack _monsterBackpack;

        [SerializeField] private Dropdown _typeDropdown;
        [SerializeField] private InputField _nameInputField;
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _removeAllButton;
        
        private void Start()
        {
            // 名前入力欄が設定されたら追加ボタンを押せるようにする
            _nameInputField.OnValueChangedAsObservable()
                .Select(x => x.Length > 0)
                .SubscribeToInteractable(_addButton)
                .AddTo(this);

            // 追加ボタンを押したら手持ちにモンスターを加える
            _addButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    var monsterType = _typeDropdown.value switch
                    {
                        0 => MonsterType.Fire,
                        1 => MonsterType.Water,
                        2 => MonsterType.Grass,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    _monsterBackpack.TryAddMonster(monsterType, _nameInputField.text);

                    _nameInputField.text = "";
                })
                .AddTo(this);

            // まとめて逃がすボタン
            _removeAllButton.OnClickAsObservable()
                .Subscribe(_ => _monsterBackpack.RemoveAllMonster())
                .AddTo(this);
        }
    }
}