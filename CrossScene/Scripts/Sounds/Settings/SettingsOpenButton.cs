using GameCore.CrossScene.Scripts.UI.Popups;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.CrossScene.Scripts.Sounds.Settings
{
    [RequireComponent(typeof(Button))]
    public class SettingsOpenButton : MonoBehaviour
    {
        [Inject, UsedImplicitly] public PopupManager popupManager { get; }
        
        private Button _buttonCached;
        public Button button
        {
            get
            {
                if (_buttonCached == null)
                    _buttonCached = GetComponent<Button>();
                return _buttonCached;
            }
        }

        private void OnEnable()
        {
            button.onClick.AddListener(Show);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(Show);
        }

        private void Show()
        {
            if(popupManager.TryGet<SoundSettingsPopup>(out var popup))
                popup.Show();
        }
    }
}