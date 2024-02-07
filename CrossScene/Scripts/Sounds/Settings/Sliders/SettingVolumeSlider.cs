using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.CrossScene.Scripts.Sounds.Settings.Sliders
{
    public abstract class SettingVolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        
        [Inject, UsedImplicitly] public SoundManager soundManager { get; }
        
        private void OnEnable()
        {
            _slider.value = GetVolume();
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            SetVolume(value);
        }

        protected abstract float GetVolume();
        protected abstract void SetVolume(float volume);
    }
}