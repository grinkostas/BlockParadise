using GameCore.CrossScene.Scripts.Saves;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.Sounds.Settings.Saves
{
    public class SoundsSaveData : SaveData<SoundsSaveData.Data>
    {
        public float musicVolume
        {
            get => savedValue.musicVolume;
            set
            {
                savedValue.musicVolume = value;
                Save();
            }
        }

        public float soundsVolume
        {
            get => savedValue.soundsVolume;
            set
            {
                savedValue.soundsVolume = value;
                Save();
            }
        }
        
        public SoundsSaveData(string key, Data defaultValue = default) : base(key, defaultValue)
        {
        }
        
        [ES3Serializable]
        public class Data
        {
            public float musicVolume;
            public float soundsVolume;

            public Data()
            {
                musicVolume = 0.5f;
                soundsVolume = 1;
            }
        }
    }
}