using DG.Tweening;
using GameCore.CrossScene.Scripts.Animations;
using GameCore.CrossScene.Scripts.Fx;
using GameCore.CrossScene.Scripts.Sounds.SoundPools;
using GameCore.CrossScene.Scripts.UI;
using GameCore.Meta.Scripts.Currencies.Models;
using GameCore.Meta.Scripts.Currencies.Views.Presenters;
using GameCore.Meta.Scripts.Pools;
using GameCore.Puzzle.Scripts.Level;
using Haptic;
using JetBrains.Annotations;
using NaughtyAttributes;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Meta.Scripts.Level
{
    public class StarLevelCompleteCollect : LevelCollectItem
    {
        [SerializeField] private RectTransform _startPoint;
        [SerializeField] private float _delay;
        [SerializeField] private float _returnStarDelay;

        [Inject, UsedImplicitly] public StarPool starPool { get; }
        [Inject, UsedImplicitly] public StarCurrencyPresenter presenter{ get; }
        [Inject, UsedImplicitly] public StarCollectModel collectModel{ get; }
        
        private void OnDisable()
        {
            DOTween.Kill(this);
        }

        private void Start()
        {
            if(completeData.finished == false || completeData.starCollected <= 0 || completeData.starsHandled)
                return;
            MoveAndCollect();
        }
        
        public override bool NeedToShow()
        {
            return (completeData.finished == false || completeData.starCollected <= 0 || completeData.starsHandled) == false;
        }

        [Button()]
        private void MoveAndCollect()
        {
            started.Dispatch();
            DOVirtual.DelayedCall(_delay, () =>
            {
                Move().OnComplete(()=>
                {
                    Collect();
                    ended.Dispatch();
                });
            }).SetLink(gameObject).SetUpdate(false);
        }

        [Button()]
        private Tween Move()
        {
            var destination = presenter.mainDisplay.icon.position;
            var star = GetStar();

            DOVirtual.DelayedCall(0.75f, returnFigureSound.PlaySound).SetId(this).SetUpdate(false);
            return flyAnimation.MoveWithScaleAndRotation(star.rectTransform, destination, 2.5f, 1, 0.35f).SetId(this)
                .OnComplete(()=>
                {
                    hapticService?.Selection();
                    ReturnStar(star);
                }).SetLink(gameObject);
        }

        private void ReturnStar(CanvasAnimateItem animateItem)
        {
            animateItem.canvasGroup.alpha = 0.0f;
            animateItem.Pool.Return(animateItem);
        }

        private CanvasAnimateItem GetStar()
        {
            var star = starPool.Get();
            star.rectTransform.position = _startPoint.position;
            star.rectTransform.localScale = new Vector3();

            return star;
        }

        private void Collect()
        {
            completeEffect.Play(presenter.mainDisplay.icon.position);
            if (completeData.starCollected <= 0) return;
            collectModel.Earn(1);
            completeData.starCollected--;
            
            if (completeData.starCollected <= 0)
                completeData.starsHandled = true;
        }
        
    }
}