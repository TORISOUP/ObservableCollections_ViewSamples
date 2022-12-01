using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using UnityEngine;
using UnityEngine.UI;

namespace Samples
{
public class SynchronizedSortedViewSample : MonoBehaviour
{
    public Button prefab;
    public GameObject root;
    ObservableFixedSizeRingBuffer<NameTag> _model;
    ISynchronizedView<NameTag, GameObject> _synchronizedView;
    private readonly int MaxCapacity = 5;

    void Start()
    {
        _model = new ObservableFixedSizeRingBuffer<NameTag>(MaxCapacity);

        // Modelに要素が増えたらViewを追加する
        _synchronizedView = _model.CreateSortedView(
            x => $"{x.Index}:{x.Name}",
            AddView,
            new NameTagComparer());

        // フィルターを設定
        _synchronizedView.AttachFilter(new GameObjectRemoveFilter<NameTag>());

        _synchronizedView.CollectionStateChanged += _ =>
        {
            foreach (var (_, view, index) in _synchronizedView.Select((x, i) =>
                         (x.Value, x.View, Index: i)))
            {
                view.transform.SetSiblingIndex(index);
            }
        };
    }

    private GameObject AddView(NameTag value)
    {
        // IObservableCollection<T>の要素が追加されたときに実行される
        var item = Instantiate(prefab, root.transform);
        item.GetComponentInChildren<Text>().text = $"{value.Index}:{value.Name}";
        return item.gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var randomName = new string(Enumerable.Repeat("あいうえおかきくけこ", 3)
                .Select(x => x[UnityEngine.Random.Range(0, 10)])
                .ToArray());
            var randomIndex = UnityEngine.Random.Range(1, 100);
            _model.AddLast(new NameTag(randomIndex, randomName));
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

// なんか適当なオブジェクト
public readonly struct NameTag : IEquatable<NameTag>
{
    public int Index { get; }
    public string Name { get; }

    public NameTag(int index, string name)
    {
        Index = index;
        Name = name;
    }

    public bool Equals(NameTag other)
    {
        return Index == other.Index && Name == other.Name;
    }

    public override bool Equals(object obj)
    {
        return obj is NameTag other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Index, Name);
    }
}


public sealed class NameTagComparer : IComparer<NameTag>
{
    public int Compare(NameTag x, NameTag y)
    {
        return x.Index - y.Index;
    }
}
}