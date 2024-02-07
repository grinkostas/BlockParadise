using System;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.Utils
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlePlaybackTimeSetter : MonoBehaviour
    {
        [SerializeField] private float _startPlaybackTime;
        
        private ParticleSystem _particleSystem;
        public ParticleSystem particleSystem
        {
            get
            {
                if (_particleSystem == null)
                    _particleSystem = GetComponent<ParticleSystem>();
                return _particleSystem;
            }
        }

        private void Start()
        {
            particleSystem.time = _startPlaybackTime;
        }
    }
}