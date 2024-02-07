using DG.Tweening;
using GameCore.CrossScene.Scripts.Animations;
using GameCore.CrossScene.Scripts.Fx;
using GameCore.CrossScene.Scripts.UI.Popups;
using GameCore.Meta.Scripts.Currencies.Models;
using GameCore.Meta.Scripts.Currencies.Views.Presenters;
using GameCore.Meta.Scripts.Pools;
using GameCore.Meta.Scripts.Zone;
using Haptic;
using JetBrains.Annotations;
using NepixSignals.Api;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.Meta.Scripts.Views
{
    public class ZoneItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _buyButton;
        [Header("Animation")] 
        [SerializeField] private float _oneItemStartMoveDelay;
        [SerializeField] private float _completeDelay;
        [SerializeField] private Bouncer _bouncer;
        [Header("Buy")] 
        [SerializeField] private float _buttonZoomOutDelay;
        [SerializeField] private float _buyDelay = 0.5f;
        [SerializeField] private Vector3 _punch;
        [SerializeField] private float _punchDuration;
        [SerializeField] private string _popupId = "StarHintPopup";
        [Header("CheckMark")] 
        [SerializeField] private RectTransform _checkMark;
        [SerializeField] private float _changeItemDelay;
        [SerializeField] private Color _color;
        [SerializeField] private Vector3 _punchRotation;
        [SerializeField] private float _rotationPunchDuration;
        [SerializeField] private int _rotationPunchVibrato;
        
        [Inject, UsedImplicitly] public Hud hud { get; }
        [Inject, UsedImplicitly] public StarPool starPool { get; }
        [Inject, UsedImplicitly] public CompleteEffect completeEffect { get; }
        [Inject, UsedImplicitly] public PopupManager popupManager { get; }
        [Inject, UsedImplicitly] public MetaZone zone { get; }
        [Inject, UsedImplicitly] public FlyAnimation flyAnimation { get; }
        [Inject, UsedImplicitly] public StarCollectModel collectModel { get; }
        [Inject, UsedImplicitly] public StarCurrencyPresenter presenter { get; }
        [InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }
        
        private Popup _popup;
        public Popup popup
        {
            get
            {
                if (_popup == null)
                    _popup = GetComponentInParent<Popup>(true);
                return _popup;
            }
        }

        private ISignalCallback _boughtCallback;
        
        private void OnEnable()
        {
            Actualize();
            _buyButton.onClick.AddListener(OnBuyButtonClick);
        }

        private void OnDisable()
        {
            _buyButton.onClick.RemoveListener(OnBuyButtonClick);
            _boughtCallback?.Off();
        }

        private void Actualize()
        {
            var nextItem = zone.nextItem;
            if (nextItem == null)
            {
                _boughtCallback?.Off();
                transform.ZoomOut();
                return;
            }
            _checkMark.gameObject.SetActive(false);
            _buyButton.transform.ZoomIn();
            _itemName.text = nextItem.itemName;
            _itemIcon.sprite = nextItem.icon;
            _priceText.text = $"{nextItem.price}";
            _boughtCallback = nextItem.bought.On(OnBought);
        }

        
        
        private void OnBought(MetaZoneItem itemView)
        {
            _boughtCallback?.Off();
            ShowCheckMark();
        }

        [NaughtyAttributes.Button]
        private void ShowCheckMark()
        {
            _checkMark.localScale = Vector3.zero;
            _checkMark.gameObject.SetActive(true);
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(_buyDelay);
            sequence.Append(_checkMark.ZoomIn());
            sequence.Join(_checkMark.DOPunchRotation(_punchRotation, _rotationPunchDuration, _rotationPunchVibrato));
            sequence.AppendCallback(() => completeEffect.Play(_checkMark.position, _color));
            sequence.AppendInterval(_changeItemDelay);
            sequence.AppendCallback(() =>
            {
                hapticService?.Selection();
                transform.DOPunchScale(_punch, _punchDuration, 2).SetUpdate(false).SetLink(gameObject);
                popup.EnableHide(this);
                _buyButton.interactable = true;
                Actualize();
            });
            sequence.SetUpdate(false).SetLink(gameObject);
        }
        
        private void OnBuyButtonClick()
        {
            var nextItem = zone.nextItem;
            if (collectModel.TryUse(nextItem.price) == false)
            {
                popup.ForceHide();
                if(popupManager.TryGet(_popupId, out var notEnoughPopup))
                    notEnoughPopup.Show();
                return;
            }
            
            _buyButton.interactable = false;
            popup.BlockHide(this);
            var sequence = DOTween.Sequence();
            for (int i = 0; i < nextItem.price; i++)
            {
                var star = starPool.Get();
                star.rectTransform.position = presenter.mainDisplay.icon.position;
                var flyTween = flyAnimation.MoveWithScaleAndRotation(star.rectTransform, _buyButton.transform.position)
                    .SetDelay(_oneItemStartMoveDelay * i)
                    .OnComplete(() =>
                    {
                        hapticService?.Selection();
                        _bouncer.Bounce();
                        completeEffect.Play(star.transform.position);
                        star.canvasGroup.alpha = 0.0f;
                        DOVirtual.DelayedCall(1.5f, () => starPool.Return(star));
                    });
                sequence.Join(flyTween);
            }
            sequence.SetLink(gameObject).SetUpdate(false);
            sequence.OnComplete(() =>
            {
                _buyButton.transform.ZoomOut().SetDelay(_buttonZoomOutDelay);
                DOVirtual.DelayedCall(_completeDelay, OnBuyComplete);
            });
        }

        private void OnBuyComplete()
        {
            popup.ForceHide(false);
            hud.Hide();
            zone.nextItem.Show().OnComplete(()=>
            {
                DOVirtual.DelayedCall(1.25f, () =>
                {
                    popup.Show();
                    hud.Show();
                    zone.BuyNextItem();
                });
            });
        }

    }
}