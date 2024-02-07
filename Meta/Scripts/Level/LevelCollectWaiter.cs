using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore.Meta.Scripts.Level
{
    public class LevelCollectWaiter : MonoBehaviour
    {
        [SerializeField] private float _additionalWaitDelay;
        [SerializeField] private List<LevelCollectItem> _levelCollectItems;
        
        private List<UnityAction> _invokeOnEnd = new();

        public int _animationsPlaying = 0;

        private void OnEnable()
        {
            foreach (var levelCollectItem in _levelCollectItems)
            {
                levelCollectItem.started.On(OnAnimationStarted);
                levelCollectItem.ended.On(OnAnimationEnded);
            }
        }
        
        private void OnDisable()
        {
            foreach (var levelCollectItem in _levelCollectItems)
            {
                levelCollectItem.started.Off(OnAnimationStarted);
                levelCollectItem.ended.Off(OnAnimationEnded);
            }
        }

        private void OnAnimationStarted()
        {
            DOTween.Kill(this);
            _animationsPlaying++;
        }
        
        private void OnAnimationEnded()
        {
            _animationsPlaying--;
            if (_animationsPlaying <= 0)
            {
                DOVirtual.DelayedCall(_additionalWaitDelay, InvokeOnEnd).ConfigureWithId(this, gameObject);
            }
        }

        public void Invoke(UnityAction action)
        {
            _invokeOnEnd.Add(action);
            if (_animationsPlaying <= 0 && DOTween.IsTweening(this) == false)
            {
                DOVirtual.DelayedCall(_additionalWaitDelay, InvokeOnEnd).SetId(this);
                return;
            }
            
        }
        
        private void InvokeOnEnd()
        {
            foreach (var action in _invokeOnEnd)
                action?.Invoke();
            _invokeOnEnd.Clear();
        }
    }
}