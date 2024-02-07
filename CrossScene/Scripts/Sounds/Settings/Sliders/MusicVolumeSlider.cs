using UnityEngine;

namespace GameCore.CrossScene.Scripts.Sounds.Settings.Sliders
{
    public class MusicVolumeSlider : SettingVolumeSlider
    {
        protected override float GetVolume()
        {
            return soundManager.settings.musicVolume;
        }

        protected override void SetVolume(float volume)
        {
            soundManager.settings.musicVolume = volume;
        }
    }
}