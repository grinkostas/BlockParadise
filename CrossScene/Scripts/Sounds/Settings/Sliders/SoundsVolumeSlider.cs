using UnityEngine;

namespace GameCore.CrossScene.Scripts.Sounds.Settings.Sliders
{
    public class SoundsVolumeSlider : SettingVolumeSlider
    {
        protected override float GetVolume()
        {
            return soundManager.settings.soundsVolume;
        }

        protected override void SetVolume(float volume)
        {
            soundManager.settings.soundsVolume = volume;
        }
    }
}