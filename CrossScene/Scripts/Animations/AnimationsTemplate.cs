using DG.Tweening;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.Animations
{
    public static class AnimationsTemplate
    {
        public static Tween ZoomIn(this Transform target, float zoom = 1.0f, float duration = 0.5f)
        {
            return target.DOScale(zoom, duration).SetEase(Ease.OutBack).SetUpdate(false);
        }
        
        public static Tween ZoomOut(this Transform target, float duration = 0.5f)
        {
            return target.DOScale(0, duration).SetEase(Ease.InBack).SetUpdate(false);
        }

        public static Tween RotateAroundAxis(this Transform target, Vector3 axis, float duration, int rotationCount = 2)
        {
            return target.DOLocalRotate(axis * (360f * rotationCount), duration, RotateMode.FastBeyond360)
                .SetRelative().SetUpdate(false);
        }
    }
}