using System;
using GameCore.CrossScene.Scripts.Saves;
using GameCore.CrossScene.Scripts.Sounds.Settings.Saves;
using UnityEngine;
using UnityEngine.Audio;

namespace GameCore.CrossScene.Scripts.Sounds.Settings
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private float _minVolume;
        
       
        private SoundsSaveData _settings;
        public SoundsSaveData settings => _settings ??= new SoundsSaveData("SoundSetting", new SoundsSaveData.Data());

        public SoundsSaveData.Data settingData => settings.savedValue;
        

        private void OnEnable()
        {
            settings.changed.On(ActualizeSetting);
        }

        private void OnDisable()
        {
            settings.changed.Off(ActualizeSetting);
        }
        
        private void Start()
        {
            ActualizeSetting();
        }

        private void ActualizeSetting()
        {
            SetVolume(SoundParameter.soundVolume, GetVolume(settingData.soundsVolume));
            SetVolume(SoundParameter.musicVolume, GetVolume(settingData.musicVolume));
        }
        
        private void SetVolume(string param, float volume)
        {
            _mixer.SetFloat(param, volume);
        }

        private float GetVolume(float percentage)
        {
            if (percentage <= 0.05)
                return -80f;
            return _minVolume - _minVolume * percentage;
        }
        
    
    }
}