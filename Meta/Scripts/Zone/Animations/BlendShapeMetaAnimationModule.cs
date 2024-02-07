using System;
using DG.Tweening;
using UnityEngine;

namespace GameCore.Meta.Scripts.Zone
{
    public class BlendShapeMetaAnimationModule : MonoBehaviour
    {
        [SerializeField] private MetaZoneItemAnimation _animation;
        [SerializeField] private float _duration;
        [SerializeField] private float _delay;
        
        private void OnEnable()
        {
            _animation.startedShowItem.On(BlendAnimate);
        }

        private void OnDisable()
        {
            _animation.startedShowItem.Off(BlendAnimate);
            DOTween.Kill(this);
        }

        private void BlendAnimate(Transform target)
        {
            if(target.TryGetComponent(out SkinnedMeshRenderer meshRenderer) == false)
                return;
            meshRenderer.SetBlendShapeWeight(0, 100);
            DOVirtual.Float(100, 0, _duration, value => meshRenderer.SetBlendShapeWeight(0, value))
                .SetDelay(_delay).SetId(this);
        }
    }
}