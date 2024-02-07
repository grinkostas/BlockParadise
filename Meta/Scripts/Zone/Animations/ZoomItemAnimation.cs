using System.Collections.Generic;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Sounds;
using GameCore.Meta.Scripts.Utils;
using NepixSignals;
using StaserSDK.Extentions;
using UnityEngine;

namespace GameCore.Meta.Scripts.Zone
{
    public class ZoomItemAnimation : MetaZoneItemAnimation
    {
        [SerializeField] private List<ZoomItem> _zoomItems;
        [Header("In")]
        [SerializeField] private float _zoomInDuration;
        [SerializeField] private Ease _inEase;
        [SerializeField] private Vector3 _targetScale;
        [SerializeField] private Vector3 _positionPunch;
        [Header("Out")]
        [SerializeField] private float _zoomOutDuration;
        [SerializeField] private Ease _outEase;
        [SerializeField] private Vector3 _fallScale;
        [SerializeField] private float _returnScaleDuration;
        
        
        [System.Serializable]
        public class ZoomItem
        {
            public List<Transform> targets;
            public List<ParticleSystem> placeParticle;
            public Vector3 pivotOffset;
            public bool waitEnd;
            public float delay;
            public float targetZoomDelay;
            public SoundPool placeSound;
        }
        
        public override Tween ShowAnimation()
        {
            var sequence = DOTween.Sequence();
            foreach (var item in _zoomItems)
            {
                sequence.AppendCallback(() => ShowItem(item));
                sequence.AppendInterval(GetItemDuration(item));
            }

            return sequence;
        }

        public override void ForceShow()
        {
            foreach (var item in _zoomItems)
            foreach (var target in item.targets)
                target.GetMeshCenterPivot(item.pivotOffset).localScale = Vector3.one;
        }

        private float GetItemDuration(ZoomItem item)
        {
            float duration = 0;
            if (item.waitEnd)
                duration += _zoomInDuration + _zoomOutDuration;
            duration += item.targetZoomDelay * Mathf.Max(0, item.targets.Count - 1);
            duration += item.delay;
            return duration;
        }
        
        private void ShowItem(ZoomItem item)
        {
            for (int i = 0; i < item.targets.Count; i++)
            {
                float delay = item.targetZoomDelay * i;
                var sequence = DOTween.Sequence();
                var targetTransform = item.targets[i].GetMeshCenterPivot(item.pivotOffset);
                var startPosition = targetTransform.localPosition;
                sequence.AppendInterval(delay);
                sequence.AppendCallback(() => startedShowItem.Dispatch(targetTransform));
                sequence.Append(targetTransform.DOLocalMove(startPosition + _positionPunch, _zoomInDuration).SetEase(_inEase));
                sequence.Join(targetTransform.DOScale(_targetScale, _zoomInDuration).SetEase(_inEase));
                sequence.Append(targetTransform.DOLocalMove(startPosition, _zoomOutDuration).SetEase(_outEase));
                sequence.Join(targetTransform.DOScale(_fallScale, _zoomOutDuration).SetEase(_outEase));
                PlayFx(item, i, ref sequence);
                sequence.AppendCallback(() => completedShowItem.Dispatch(targetTransform));
                sequence.Append(targetTransform.DOScale(Vector3.one, _returnScaleDuration));
            }
        }

        private void PlayFx(ZoomItem item, int index, ref Sequence sequence)
        {
            if (item.placeSound != null)
                sequence.AppendCallback(item.placeSound.PlaySound);
            if (item.placeParticle.Count > 0)
            {
                sequence.AppendCallback(item.placeParticle[index].Play);
            }
        }
        
        public override void Hide()
        {
            foreach (var item in _zoomItems)
                foreach (var target in item.targets)
                    target.GetMeshCenterPivot(item.pivotOffset).localScale = Vector3.zero;
        }
    }
}