using System;
using DG.Tweening;
using GameCore.CrossScene.Scripts.UI.Popups;
using GameCore.Meta.Scripts.Currencies.Views.Presenters;
using GameCore.Meta.Scripts.Level;
using GameCore.Meta.Scripts.Zone;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.Meta.Scripts.Popups
{
    public class ZoneBuyItemsPopup : TypeInjectablePopup
    {
        [SerializeField] private float _openDelay;
        [Inject, UsedImplicitly] public StarCurrencyPresenter presenter { get; }
        [Inject, UsedImplicitly] public MetaZone zone { get; }
        [Inject, UsedImplicitly] public LevelCollectWaiter levelCollectWaiter { get; }
        protected override Type popupType => typeof(ZoneBuyItemsPopup);

        private void Start()
        {
            if(zone.nextItem == null)
                return;
            levelCollectWaiter.Invoke(() =>
            {
                if (presenter.starCollectModel.amount >= zone.nextItem.price)
                    Show();
            });
            
        }

        protected override void OnShowStart()
        {
            DOTween.Kill(this);
            presenter.mainDisplay.canvas.sortingOrder = 10;
        }

        protected override void OnHideComplete()
        {
            presenter.mainDisplay.canvas.sortingOrder = -1;
        }
    }
}