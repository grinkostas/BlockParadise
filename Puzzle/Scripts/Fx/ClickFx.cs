using DG.Tweening;
using NaughtyAttributes;
using NepixSignals;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore.Puzzle.Scripts.Fx
{
    public class ClickFx : MonoBehaviour, IPoolItem<ClickFx>
    {
        [SerializeField] private RectTransform _cube1;
        [SerializeField] private RectTransform _cube2;
        [SerializeField] private Vector2 _clickSize;
        [SerializeField] private Vector2 _cubeSizeDelta;
        [SerializeField] private float _zoomDuration;
        [Space]
        [SerializeField] private CanvasGroup _graphicCanvasGroup;
        [SerializeField] private float _hideDuration;

        private RectTransform _rectTransformCached;
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransformCached == null)
                    _rectTransformCached = GetComponent<RectTransform>();
                return _rectTransformCached;
            }
        }

        public IPool<ClickFx> Pool { get; set; }
        public bool IsTook { get; set; }
        
        public TheSignal showComplete { get; } = new();

        [Button()]
        public void Show()
        {
            _graphicCanvasGroup.alpha = 1;
            
            DOTween.Kill(this);
            var sequence = DOTween.Sequence();
            sequence.Append(ShowCube(_cube1));
            sequence.Join(ShowCube(_cube2));
            sequence.Append(_cube1.DOSizeDelta(_clickSize-_cubeSizeDelta, _zoomDuration));
            sequence.Join(_cube2.DOSizeDelta(_clickSize+_cubeSizeDelta, _zoomDuration));
            sequence.Join(_graphicCanvasGroup.DOFade(0, _hideDuration).SetDelay(_zoomDuration-_hideDuration));
            sequence.AppendCallback(showComplete.Dispatch);
            sequence.SetId(this).SetLink(gameObject);
        }


        private Tween ShowCube(RectTransform cube)
        {
            cube.sizeDelta = Vector2.zero;
            return cube.DOSizeDelta(_clickSize, _zoomDuration);
        }
        
        public void TakeItem()
        {
        }

        public void ReturnItem()
        {
        }
    }
}