using DG.Tweening;
using StaserSDK.Extentions;
using UnityEngine;

namespace GameCore.Meta.Scripts.Utils
{
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class BlendShapeAnimator : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _delay;
        [SerializeField] private bool _inverted;
        [SerializeField] private Vector2 _range;

        private SkinnedMeshRenderer _meshRendererCached;
        public SkinnedMeshRenderer renderer
        {
            get
            {
                if (_meshRendererCached == null)
                    _meshRendererCached = GetComponent<SkinnedMeshRenderer>();
                return _meshRendererCached;
            }
        }
        
        private void OnEnable()
        {
            var sequence = DOTween.Sequence();
            Prepare();
            sequence.AppendInterval(_delay);
            sequence.Append(GetAnimationTween().SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo));
            sequence.ConfigureWithId(this, gameObject);
        }

        private void Prepare()
        {
            renderer.SetBlendShapeWeight(0, _inverted ? _range.y : _range.x);
        }
        
        private Tween GetAnimationTween()
        {
            return _inverted ? AnimateInverted() : Animate();
        }
        
        private Tween Animate()
        {
            return DOVirtual.Float(_range.x, _range.y, _duration, value =>
            {
                renderer.SetBlendShapeWeight(0, value);
            });
        }

        private Tween AnimateInverted()
        {
            return DOVirtual.Float(_range.y, _range.x, _duration, value =>
            {
                renderer.SetBlendShapeWeight(0, value);
            });
        }
        
        private void OnDisable()
        {
            DOTween.Kill(this);
        }
    }
}