using System;
using GameCore.CrossScene.Scripts.UI.Popups;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.CrossScene.Scripts.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class PopupCloseButton : MonoBehaviour
    {
        private Button _button;
        public Button button
        {
            get
            {
                if (_button == null)
                    _button = GetComponent<Button>();
                return _button;
            }
        }

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

        private void OnEnable()
        {
            button.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(Hide);
        }

        private void Hide() => popup.Hide();
    
    }
}