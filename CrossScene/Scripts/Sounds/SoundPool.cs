using UnityEngine;

namespace GameCore.CrossScene.Scripts.Sounds
{
    [RequireComponent(typeof(SoundItem))]
    public class SoundPool : MonoBehaviour
    {
        private SoundItem _soundItemCached;
        private SoundItem soundItem
        {
            get
            {
                if (_soundItemCached == null)
                    _soundItemCached = GetComponent<SoundItem>();
                return _soundItemCached;
            }
        }
        
        private SimplePool<SoundItem> _soundPool;
        private void Awake()
        {
            InitPool();
        }

        public void PlaySound()
        {
            InitPool();
            _soundPool.Get().Play();
        }

        public SoundItem Get()
        {
            InitPool();
            return _soundPool.Get();
        }

        private void InitPool()
        {
            if (_soundPool == null)
            {
                _soundPool = new SimplePool<SoundItem>(soundItem, 0, transform)
                {
                    disableOnReturn = false
                };
                _soundPool.AddItem(soundItem);
            }
            
            if(_soundPool.initialized == false)
                _soundPool.Initialize();
        }
    }
}