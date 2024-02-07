using System.Collections.Generic;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.Fx
{
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(AudioSource))]
    public class SoundParticle : MonoBehaviour
    {
        [SerializeField] private List<AudioSource> _additionalSounds;
        
        private ParticleSystem _particleSystemCached;
        public ParticleSystem system
        {
            get
            {
                if (_particleSystemCached == null)
                    _particleSystemCached = GetComponent<ParticleSystem>();
                return _particleSystemCached;
            }
        }

        private AudioSource _audioSourceCached;
        public AudioSource audio
        {
            get
            {
                if (_audioSourceCached == null)
                    _audioSourceCached = GetComponent<AudioSource>();
                return _audioSourceCached;
            }
        }

        public void Play()
        {
            system.Play();
            audio.Play();
            foreach (var sound in _additionalSounds)
                sound.Play();
        }
    }
}