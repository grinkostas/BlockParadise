using System.Collections.Generic;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Animations;
using GameCore.CrossScene.Scripts.Sounds;
using GameCore.CrossScene.Scripts.Sounds.SoundPools;
using GameCore.Meta.Scripts.Utils;
using JetBrains.Annotations;
using StaserSDK.Extentions;
using UnityEngine;
using Zenject;

namespace GameCore.Meta.Scripts.Zone
{
    public class ClearHouseAnimation : MetaZoneItemAnimation
    {
        [Header("House")] 
        [SerializeField] private Transform _houseParent;
        [SerializeField] private Transform _houseBase;
        [SerializeField] private Transform _houseRoof;
        [SerializeField] private Vector3 _housePositionPunch;
        [SerializeField] private Vector3 _houseRoofPositionPunch;
        [SerializeField] private float _houseBaseShowDelay;
        [SerializeField] private float _houseOutDuration;
        [SerializeField] private SoundPool _houseSound;
        [SerializeField] private ParticleSystem _housePlaceParticle;
        [Header("Platform")] 
        [SerializeField] private Transform _platformBase;
        [SerializeField] private List<Transform> _platformParts;
        [SerializeField] private float _platformItemScale;
        [SerializeField] private Vector3 _platformPositionPunch;
        [SerializeField] private float _partShowDelay;
        [SerializeField] private SoundPool _platformPlankSound;
        [Header("Tents")] 
        [SerializeField] private List<Transform> _tents;
        [SerializeField] private float _tentsShowDelay;
        [Header("Animation")] 
        [SerializeField] private Vector3 _fallScale;
        [SerializeField] private float _zoomInDuration;
        [SerializeField] private float _zoomOutDuration;
        
        private List<Transform> _allTransforms = new();

        private List<Transform> allTransforms
        {
            get
            {
                if (_allTransforms.Count == 0)
                {
                    _allTransforms.Add(_houseBase);
                    _allTransforms.Add(_houseRoof);
                    _allTransforms.Add(_platformBase);
                    _allTransforms.AddRange(_platformParts);
                    _allTransforms.AddRange(_tents);
                }

                return _allTransforms;
            }
        }


        public override Tween ShowAnimation()
        {
            var sequence = DOTween.Sequence();
            _platformBase.localScale = new Vector3(0, 0, 0);
            sequence.Append(_platformBase.DOScaleZ(1, _zoomInDuration).SetEase(Ease.OutBack));
            sequence.Append(_platformBase.DOScaleX(1, 0.1f).SetEase(Ease.OutBack));
            sequence.Append(_platformBase.DOScaleY(1, 0.1f).SetEase(Ease.OutBack));

            for (int i = 0; i < _platformParts.Count; i++)
            {
                sequence.Join(_platformParts[i].JumpUpWithScale(_platformPositionPunch, _platformItemScale,
                    _zoomInDuration, _zoomOutDuration).SetDelay(_partShowDelay * (i + 1))
                    .OnComplete(()=>_platformPlankSound.PlaySound()));
            }

            var houseParentStartPosition = _houseParent.transform.localPosition;
            sequence.Append(_houseParent.DOLocalMove(houseParentStartPosition + _housePositionPunch, _zoomInDuration));
            sequence.Join(_houseParent.DOScale(1.5f, _zoomInDuration).SetEase(Ease.OutBack));
            sequence.Join(_houseBase.DOScale(1, _zoomInDuration).SetEase(Ease.OutBack));
            sequence.AppendInterval(_houseBaseShowDelay);
            sequence.Join(_houseRoof.JumpUpWithScale(_houseRoofPositionPunch, 1, _zoomInDuration, _zoomOutDuration, 1, Ease.InQuad)
                .OnComplete(()=>_houseSound.PlaySound()));
            
            sequence.AppendInterval(_tentsShowDelay);
            foreach (var tent in _tents)
                sequence.Join(tent.DOScale(1, _zoomInDuration).SetEase(Ease.OutBack));
            
            sequence.Append(_houseParent.DOLocalMove(houseParentStartPosition, _houseOutDuration)
                .SetEase(Ease.InBack).OnComplete(() =>
                {
                    _houseSound.PlaySound();
                    _housePlaceParticle.Play();
                }));
            sequence.Join(_houseParent.DOScale(_fallScale, _houseOutDuration).SetEase(Ease.InQuad));
            sequence.Append(_houseParent.DOScale(1, 0.35f).SetEase(Ease.OutBack));
            

            return sequence;
        }

        public override void ForceShow()
        {
            foreach (var target in allTransforms)
                target.localScale = Vector3.one;
        }

        public override void Hide()
        {
            foreach (var target in allTransforms)
                target.localScale = Vector3.zero;
            
        }
    }
}