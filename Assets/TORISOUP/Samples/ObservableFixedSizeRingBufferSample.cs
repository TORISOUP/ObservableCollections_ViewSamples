using System;
using System.Collections.Specialized;
using ObservableCollections;
using UnityEngine;

namespace Samples
{
    public class ObservableFixedSizeRingBufferSample : MonoBehaviour
    {
        private ObservableFixedSizeRingBuffer<int> _ringBuffer;

        private void Start()
        {
            // Capacity:3 の固定長RingBufferとして生成する
            _ringBuffer = new ObservableFixedSizeRingBuffer<int>(3);

            // コレクション変動イベントを購読
            // NotifyCollectionChangedEventArgs が要素として渡ってくる
            _ringBuffer.CollectionChanged += (in NotifyCollectionChangedEventArgs<int> args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        Debug.Log($"Add:[{args.NewStartingIndex}] = {args.NewItem}");
                        break;
                    case NotifyCollectionChangedAction.Move:
                        Debug.Log(
                            $"Move:[{args.OldStartingIndex}] => [{args.NewStartingIndex}]");
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        Debug.Log($"Remove:[{args.OldStartingIndex}] = {args.OldItem}");
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        Debug.Log(
                            $"Replace:[{args.OldStartingIndex}] = ({args.OldItem} => {args.NewItem})");
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        Debug.Log("Reset");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };



            // 末尾に追加
            _ringBuffer.AddLast(1);
            _ringBuffer.AddLast(2);
            _ringBuffer.AddLast(3);

            // 末尾削除
            _ringBuffer.RemoveLast();

            // 先頭に差し込み
            _ringBuffer.AddFirst(0);

            // 先頭に差し込み(キャパシティを超える）
            _ringBuffer.AddFirst(-1);

            // 真ん中の要素を書き換え
            _ringBuffer[1] = 10;
        }
    }
}