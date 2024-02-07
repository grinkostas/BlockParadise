using DG.Tweening;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.Animations
{
    public class FlyAnimation : MonoBehaviour
    {
        [SerializeField] private float _moveDuration = 0.75f;
        [SerializeField] private Ease _moveEase = Ease.InOutSine;
        [Header("Offset")]
        [SerializeField] private float _offsetInDuration = 0.45f;
        [SerializeField] private float _offsetOutDuration = 0.3f;
        [SerializeField] private float _offset = 15;
        [SerializeField] private Ease _offsetInEase = Ease.OutCirc;
        [SerializeField] private Ease _offsetOutEase = Ease.InCirc;

        public float duration => _moveDuration;
        
        public Tween Move(RectTransform view, Vector3 destination)
        {
            Vector3 startPosition = view.transform.position;
            float offsetX = 0.0f;
            
            int direction = startPosition.x > destination.x ? 1 : -1;
            float offset = _offset * direction;
            
            return DOVirtual.Float(0, 1, _moveDuration, progress =>
            {
                Vector3 targetPosition = Vector3.Lerp(startPosition, destination, progress);
                
                float wastedTime = progress * _moveDuration;
                if (progress <= _offsetInDuration / _moveDuration)
                {
                    float offsetProgress = wastedTime / _offsetInDuration;
                    offsetX = DOVirtual.EasedValue(0, offset, offsetProgress, _offsetInEase);
                }
                else
                {
                    float offsetProgress = (wastedTime-_offsetInDuration) / _offsetOutDuration;
                    if (offsetProgress >= 1)
                        offsetX = 0;
                    else
                        offsetX = DOVirtual.EasedValue(offset, 0, offsetProgress, _offsetOutEase);
                }

                targetPosition.x += offsetX;
                view.transform.position = targetPosition;
            }).SetEase(_moveEase).SetLink(view.gameObject).SetId(view);
        }

        public Tween MoveWithScaleAndRotation(RectTransform view, Vector3 destination)
        {
            view.transform.localScale = Vector3.zero;
            var sequence = DOTween.Sequence();
            sequence.Append(view.transform.ZoomIn());
            sequence.Append(MoveWithRotation(view, destination));
            sequence.SetUpdate(false);
            
            return sequence;
        }

        public Tween MoveWithRotation(RectTransform view, Vector3 destination)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(Move(view, destination));
            sequence.Join(view.transform.RotateAroundAxis(new Vector3(0, 0, 1), _moveDuration));
            sequence.SetUpdate(false);
            return sequence;
        }

        public Tween MoveWithScaleAndRotation(RectTransform view, Vector3 destination, float startScale, float endScale,
            float startFlyDelay = 0.0f)
        {
            view.transform.localScale = Vector3.zero;
            var sequence = DOTween.Sequence();
            sequence.Append(view.transform.ZoomIn(startScale));
            sequence.AppendInterval(startFlyDelay);
            sequence.Append(MoveWithRotation(view, destination));
            sequence.Join(view.transform.DOScale(endScale, _moveDuration));
            sequence.SetUpdate(false);
            
            return sequence;
        }

        public void Kill(RectTransform rectTransform)
        {
            DOTween.Kill(rectTransform);
        }
    }
}