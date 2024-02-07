using System;
using GameCore.CrossScene.Scripts.UI.Popups;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.CrossScene.Scripts.UI.Buttons
{
    public class ShowPopupButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _popupId;

        [Inject, UsedImplicitly] public PopupManager popupManager { get; }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (popupManager.TryGet(_popupId, out Popup popup))
                popup.Show();
        }
    }
}