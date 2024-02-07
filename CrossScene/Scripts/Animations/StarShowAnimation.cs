using System;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Sounds.SoundPools;
using JetBrains.Annotations;
using StaserSDK.Extentions;
using UnityEngine;
using Zenject;

namespace GameCore.CrossScene.Scripts.Animations
{
    public class StarShowAnimation : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _star;
        [SerializeField] private Transform _ribbonWrapper;
        [SerializeField] private Transform _ribbon;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private float _startScale;
        [SerializeField] private Ease _zoomEase;
        [SerializeField] private float _duration;
        [SerializeField] private float _particlePlayDelay;
        [SerializeField] private ParticleSystem _particleSystem;
        
        [Inject, UsedImplicitly] public LineDestroySoundPool destroySound { get; }
        
        private void OnEnable()
        {
            _canvasGroup.alpha = 0;
            _star.localScale = Vector3.one * _startScale;
            _ribbon.localScale = Vector3.zero;
            _ribbonWrapper.localScale = Vector3.one;

            _canvasGroup.DOFade(1, _fadeDuration).ConfigureWithId(this, gameObject);
            _star.DOScale(1, _duration).SetEase(_zoomEase).ConfigureWithId(this, gameObject)
                .OnComplete(()=>
                {
                    destroySound.PlaySound();
                    _ribbonWrapper.DOPunchScale(new Vector3(0.25f, 0, 0), _duration, 2);
                    _ribbon.DOScale(1, _duration).SetEase(Ease.OutBack);
                });
            _star.RotateAroundAxis(Vector3.forward, _duration, 1).ConfigureWithId(this, gameObject);
            DOVirtual.DelayedCall(_particlePlayDelay, _particleSystem.Play).SetUpdate(false);
        }

        private void OnDisable()
        {
            _canvasGroup.alpha = 1;
            _star.localScale = Vector3.one;
            _star.localRotation = Quaternion.identity;
            DOTween.Kill(this);
        }
    }
}