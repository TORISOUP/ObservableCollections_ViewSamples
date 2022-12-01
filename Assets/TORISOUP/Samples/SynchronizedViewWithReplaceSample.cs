using System.Collections.Specialized;
using ObservableCollections;
using UnityEngine;
using UnityEngine.UI;

namespace Samples
{
public class SynchronizedViewWithReplaceSample : MonoBehaviour
{
    public Button prefab;
    public GameObject root;
    ObservableFixedSizeRingBuffer<int> _model;
    ISynchronizedView<int, GameObject> _synchronizedView;

    private readonly int MaxCapacity = 5;

    void Start()
    {
        _model = new ObservableFixedSizeRingBuffer<int>(MaxCapacity);

        // Modelに要素が増えたらViewを追加する
        _synchronizedView = _model.CreateView(AddView);

        // 要素を消す&Replaceするフィルターを設定
        _synchronizedView.AttachFilter(new FilterMux<int, GameObject>(
            new GameObjectRemoveFilter<int>(),
            new ReplaceFilter()));
    }

    private GameObject AddView(int value)
    {
        // IObservableCollection<T>の要素が追加されたときに実行される
        var item = Instantiate(prefab, root.transform);
        item.GetComponentInChildren<Text>().text = value.ToString();
        return item.gameObject;
    }

    private void Update()
    {
        // キーを押すごとに要素を1つ追加していく
        if (Input.GetKeyDown(KeyCode.A))
        {
            _model.AddLast(Time.frameCount);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // とりあえず真ん中の要素を0で上書きする
            _model[2] = 0;
        }
    }

    void OnDestroy()
    {
        if (_synchronizedView == null) return;

        // Destroy時に紐づいたViewも一緒に消す
        foreach (var (_, view) in _synchronizedView)
        {
            if (view != null) Destroy(view);
        }

        _synchronizedView.Dispose();
    }
}

public class ReplaceFilter : ISynchronizedViewFilter<int, GameObject>
{
    public bool IsMatch(int value, GameObject view)
    {
        return true;
    }

    public void WhenTrue(int value, GameObject view)
    {
    }

    public void WhenFalse(int value, GameObject view)
    {
    }

    public void OnCollectionChanged(ChangedKind changedKind,
        int value,
        GameObject view,
        in NotifyCollectionChangedEventArgs<int> eventArgs)
    {
        // eventArgs.Actionは「大本のコレクションで何が起きたか？」
        // ここではReplaceに限定する
        if (eventArgs.Action != NotifyCollectionChangedAction.Replace) return;

        // Replace起因でViewのAddが発生したことがここでわかるので、
        // その場合にViewの要素の並び替えを行う
        if (changedKind == ChangedKind.Add)
        {
            // ヒエラルキーの順序がViewの並び順に一致している
            view.transform.SetSiblingIndex(eventArgs.NewStartingIndex);
        }
    }
}
}