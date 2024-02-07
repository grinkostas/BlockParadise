using System;
using GameCore.CrossScene.Scripts.UI.Popups;
using GameCore.Meta.Scripts.Currencies.Models;
using GameCore.Meta.Scripts.Popups;
using GameCore.Meta.Scripts.Zone;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.Meta.Scripts.Buttons
{
    public class ZoneButton : MonoBehaviour
    {
        [SerializeField] private SimpleSlider _progressSlider;
        [SerializeField] private TMP_Text _progressText;
        [SerializeField] private Button _button;
        [Header("Available")] 
        [SerializeField] private GameObject _availableContainer;
        private Popup _popup;
        
        [Inject, UsedImplicitly] public MetaZone zone { get; }
        [Inject, UsedImplicitly] public PopupManager popupManager { get; }
        [Inject, UsedImplicitly] public StarCollectModel starCollectModel { get; }

        private void OnEnable()
        {
            Actualize(true);
            zone.changed.On(Actualize);
            starCollectModel.onChange.On(OnChangeCollectModel);
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
            starCollectModel.onChange.Off(OnChangeCollectModel);
            zone.changed.Off(Actualize);
        }

        private void OnChangeCollectModel(float amount) => Actualize();

        private void Actualize() => Actualize(false);
        private void Actualize(bool force)
        {
            if(force)
                _progressSlider.ForceActualize(zone.progress);
            else
                _progressSlider.value = zone.progress;
            _progressText.text = $"{zone.completedItemsCount}/{zone.totalItemsCount}";
            ActualizeAvailable();
        }

        private void ActualizeAvailable()
        {
            if (zone.nextItem == null)
            {
                _availableContainer.SetActive(false);
                return;
            }

            _availableContainer.SetActive(starCollectModel.amount >= zone.nextItem.price);
        }

        private void OnClick()
        {
            if(popupManager.TryGet<ZoneBuyItemsPopup>(out _popup) == false)
                return;
            _popup.Show();
        }
    }
}