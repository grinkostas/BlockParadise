using System;
using Cinemachine;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Fx;
using NaughtyAttributes;
using NepixSignals;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore.Meta.Scripts.Zone
{
    public abstract class MetaZoneItemAnimation : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _showCamera;
        [SerializeField] private float _endDelay;
        [SerializeField, ShowIf(nameof(_showParticle))] private SoundParticle _particle;
        [SerializeField] private bool _showParticle;

        public TheSignal<Transform> startedShowItem { get; } = new();
        public TheSignal<Transform> completedShowItem { get; } = new();
        
        private void Awake()
        {
            HideCamera();
        }

        [Button()]
        public Tween Show()
        {
            ShowCamera();
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(0.65f);
            sequence.Append(ShowAnimation());
            sequence.AppendCallback(ShowParticle);
            sequence.AppendInterval(_endDelay);
            sequence.AppendCallback(HideCamera);
            return sequence;
        }
        public abstract Tween ShowAnimation();
        public abstract void ForceShow();
        
        [Button()]
        public abstract void Hide();

        public void ShowParticle()
        {
            if(_showParticle)
                _particle.Play();
        }

        private void ShowCamera()
        {
            if(_showCamera != null)
                _showCamera.gameObject.SetActive(true);
        }

        private void HideCamera()
        {
            if(_showCamera != null)
                _showCamera.gameObject.SetActive(false);
        }
    }
}