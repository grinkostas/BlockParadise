using GameCore.CrossScene.Scripts.Animations;
using GameCore.Meta.Scripts.Currencies.Views.Presenters;
using NaughtyAttributes;
using StaserSDK;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameCore.Meta.Scripts.Currencies.Views.Displays
{
    [RequireComponent(typeof(CooldownPulseAnimation))]
    public abstract class CurrencyDisplay : MonoBehaviour
    {
        [SerializeField] private RectTransform _iconRect;
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private bool _mainDisplay;

        private CooldownPulseAnimation _pulseAnimation;
        public CooldownPulseAnimation pulseAnimation => _pulseAnimation ??= GetComponent<CooldownPulseAnimation>();
        
        public Canvas canvas => _canvas;
        public RectTransform icon => _iconRect;
        
        public abstract CurrencyPresenter currencyPresenter { get; }
        
        [Inject]
        public void Construct()
        {
            currencyPresenter.AddDisplay(this, _mainDisplay);
        }

        public void SetAmount(float amount)
        {
            _amountText.text = FormatAmount(amount);
            pulseAnimation.Pulse();
            OnSetAmount(amount);
        }

        protected virtual string FormatAmount(float amount)
        {
            return Mathf.CeilToInt(amount).ToString();
        }

        protected virtual void OnSetAmount(float amount)
        {
        }

        [Button()]
        public void DebugAdd()
        {
            currencyPresenter.model.Earn(1);
        }
        
    }
}