using DG.Tweening;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundItem : MonoBehaviour, IPoolItem<SoundItem>
    { 
        private AudioSource _audioSourceCached;
        public AudioSource source
        {
            get
            {
                if (_audioSourceCached == null)
                    _audioSourceCached = GetComponent<AudioSource>();;
                return _audioSourceCached;
            }
        }

        public IPool<SoundItem> Pool { get; set; }
        public bool IsTook { get; set; }

        public void Play()
        {
            if(source == null || source.clip == null)
                return;
            source.Play();
            DOVirtual.DelayedCall(source.clip.length, () => Pool.Return(this)).SetLink(gameObject);
        }
        
        public void TakeItem()
        {
        }

        public void ReturnItem()
        {
            source.pitch = 1.0f;
            source.Stop();
        }
    }
}