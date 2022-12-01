using ObservableCollections;
using UnityEngine;
using UnityEngine.UI;

namespace Samples
{
    public class SynchronizedViewSample : MonoBehaviour
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

            // 要素を消すフィルターを追加
            _synchronizedView.AttachFilter(new GameObjectRemoveFilter<int>());
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
}