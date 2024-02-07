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
    public class CoinsLevelCollect : LevelCollectItem
    {
        [SerializeField] private LevelCollectItem _previousCollectItem;
        [SerializeField] private RectTransform _startPoint;
        [SerializeField] private float _delay;
        [SerializeField] private float _returnItemDelay;
        [SerializeField] private int _scoreToCoin;
        [Header("Animation")] 
        [SerializeField] private float _zoomDuration;
        [SerializeField] private float _sphereRadius;
        [SerializeField] private int _maxCoinsCount;
        [SerializeField] private float _startMoveItemDelay;
        [SerializeField] private float _moveDuration;

        [Inject, UsedImplicitly] public CoinsPool coinsPool { get; }
        [Inject, UsedImplicitly] public SoftCurrencyPresenter presenter{ get; }
        [Inject, UsedImplicitly] public SoftCurrencyModel collectModel{ get; }
        
        
        private void OnDisable()
        {
            DOTween.Kill(this);
        }

        private void Start()
        {
            if(NeedToShow() == false)
                return;
            if (_previousCollectItem.NeedToShow())
            {
                _previousCollectItem.ended.Once(MoveAndCollect);
                return;
            }
            MoveAndCollect();
        }
        
        public override bool NeedToShow()
        {
            return (completeData.finished == false || completeData.score <= 0 || completeData.scoreHandled) == false;
        }

        [Button()]
        private void MoveAndCollect()
        {
            started.Dispatch();
            DOVirtual.DelayedCall(_delay, () =>
            {
                Move(completeData.score/_scoreToCoin);
            }).SetLink(gameObject).SetUpdate(false);
        }

        [Button()]
        private void Move() => Move(6);
        private void Move(int reward)
        {
            var destination = presenter.mainDisplay.icon.position;
            int coinsItemCount = Mathf.Min(reward, _maxCoinsCount);
            if (coinsItemCount <= 0)
                return;
            
            int itemReward = reward / coinsItemCount;
            int firstItemReward = reward - itemReward * (coinsItemCount-1);
            var sequence = DOTween.Sequence();
            sequence.Append(MoveCoin(destination, firstItemReward));
            for (int i = 1; i < coinsItemCount; i++)
            {
                sequence.Join(MoveCoin(destination, itemReward).SetDelay(i * _startMoveItemDelay));
            }

            sequence.OnComplete(() =>
            {
                if (completeData == null)
                    return;
                completeData.scoreHandled = true;
                ended.Dispatch();
            });
            sequence.SetLink(gameObject);
        }

        private Tween MoveCoin(Vector3 destination, int reward)
        {
            var coin = GetCoin();
            coin.rectTransform.localScale = Vector3.zero;
            coin.rectTransform.position = _startPoint.position + (Random.insideUnitCircle * _sphereRadius).XY();
            var sequence = DOTween.Sequence();
            coin.rectTransform.RotateAroundAxis(new Vector3(0, 0, 1), _moveDuration + _zoomDuration, 3).SetUpdate(false).SetLink(gameObject);
            sequence.Append(coin.transform.ZoomIn(duration:_zoomDuration));
            sequence.AppendCallback(returnFigureSound.PlaySound);
            sequence.Append(coin.rectTransform.DOMove(destination, _moveDuration).SetEase(Ease.InBack));
            sequence.SetUpdate(false).SetLink(gameObject);
            sequence.OnComplete(() =>
            {
                hapticService?.Selection();
                Collect(reward);
                ReturnCoin(coin);
            });
            return sequence;
        }

        private void ReturnCoin(CanvasAnimateItem animateItem)
        {
            animateItem.canvasGroup.alpha = 0.0f;
            DOVirtual.DelayedCall(_returnItemDelay, () =>
            {
                animateItem.Pool.Return(animateItem);
            }).SetUpdate(false).SetId(this).SetLink(gameObject);
        }

        private CanvasAnimateItem GetCoin()
        {
            var star = coinsPool.Get();
            star.rectTransform.position = _startPoint.position;
            star.rectTransform.localScale = new Vector3();

            return star;
        }

        private void Collect(int amount)
        {
            if(completeData == null || completeData.scoreHandled)
                return;
            
            if (completeData.score > 0)
            {
                collectModel.Earn(amount);
                completeData.score -= amount * _scoreToCoin;
            }
        }

        
    }
}