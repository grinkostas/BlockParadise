using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Fx
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ColoredParticle : MonoBehaviour, IPoolItem<ColoredParticle>
    {
        private List<ParticleSystem> _particlesCached = new();

        public List<ParticleSystem> particles
        {
            get
            {
                if (_particlesCached.Count != 0) return _particlesCached;
                
                _particlesCached.Add(GetComponent<ParticleSystem>());
                _particlesCached.AddRange(GetComponentsInChildren<ParticleSystem>());

                return _particlesCached;
            }
        }

        public IPool<ColoredParticle> Pool { get; set; }
        public bool IsTook { get; set; }
        
        public void Play(Color color)
        {
            SetColor(color);
            Play();
        }
        
        public void Play()
        {
            particles[0].Play();
        }

        public void SetColor(Color color)
        {
            foreach (var particle in particles)
            {
                var particleMain = particle.main;
                particleMain.startColor = new ParticleSystem.MinMaxGradient(color);
            }
        }

        
        public void TakeItem()
        {
        }

        public void ReturnItem()
        {
        }
    }
}