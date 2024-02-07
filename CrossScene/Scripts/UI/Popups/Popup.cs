using System;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Animations;
using JetBrains.Annotations;
using NaughtyAttributes;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.CrossScene.Scripts.UI.Popups
{
    public abstract class Popup : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private GameObject _content;
        [SerializeField] private CanvasGroup _overlay;
        [SerializeField] private CanvasGroup _graphic;
        
        [Inject, UsedImplicitly] public PopupManager popupManager { get; }

        private List<object> _hideBlockers = new();

        public string id => _id;
        
        public TheSignal showStarted { get; } = new();
        public TheSignal showCompleted { get; } = new();
        public TheSignal hideStarted { get; } = new();
        public TheSignal hideCompleted { get; } = new();

        [Inject]
        protected virtual void Inject()
        {
            popupManager.Add(_id, this);
            OnInject();
        }
        
        protected virtual void OnInject(){}

        private void Awake()
        {
            _content.SetActive(false);
            _overlay.alpha = 0f;
            _graphic.alpha = 0f;
        }

        [Button()]
        public virtual void Show()
        {
            OnShowStart();
            showStarted.Dispatch();
            DOTween.Kill(this);
            _content.SetActive(true);
            var sequence = DOTween.Sequence();
            sequence.Join(_overlay.DOFade(1, 0.2f).SetEase(Ease.Linear));
            sequence.Join(_graphic.DOFade(1, 0.15f).SetEase(Ease.Linear));
            sequence.Join(_graphic.transform.DOPunchScale(Vector3.one * 0.25f, 0.35f, 3));
            sequence.OnComplete(()=>
            {
                OnShowComplete();
                showCompleted.Dispatch();
            });
            sequence.SetLink(gameObject).SetId(this);
        }

        public void ForceHide(bool disable = true)
        {
            hideStarted.Dispatch();
            if(disable)
                _content.SetActive(false);
            _graphic.alpha = 0.0f;
            _overlay.alpha = 0.0f;
            hideCompleted.Dispatch();
        }

        protected virtual void OnShowStart(){}
        protected virtual void OnShowComplete(){}

        [Button()]
        public virtual void Hide()
        {
            if(_hideBlockers.Count > 0)
                return;
            
            OnHideStart();
            hideStarted.Dispatch();
            DOTween.Kill(this);
            var sequence = DOTween.Sequence();
            sequence.Join(_overlay.DOFade(0, 0.15f));
            sequence.Join(_graphic.DOFade(0, 0.2f).SetEase(Ease.Linear));
            sequence.OnComplete(() =>
            {
                OnHideComplete();
                hideCompleted.Dispatch();
                _content.SetActive(false);
            });
            sequence.SetLink(gameObject).SetId(this);
        }
        
        protected virtual void OnHideStart(){}
        protected virtual void OnHideComplete(){}

        public void BlockHide(object sender)
        {
            if(_hideBlockers.Contains(sender))
                return;
            _hideBlockers.Add(sender);
        }

        public void EnableHide(object sender)
        {
            _hideBlockers.Remove(sender);
        }
    }
}