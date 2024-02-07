using DG.Tweening;
using GameCore.CrossScene.Scripts.Sounds.SoundPools;
using GameCore.Puzzle.Scripts.Fx;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.CrossScene.Scripts.Fx
{
    public class CompleteEffect : MonoBehaviour
    {
        [SerializeField] private MonoPool<ColoredParticle> _pool;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private float _scale = 50f;
        [SerializeField] private float _returnDelay;
        
        [Inject, UsedImplicitly] public CompleteSoundPool soundPool { get; }

        public void Play(Vector3 position) => Play(position, _defaultColor);
        
        public void Play(Vector3 position, Color color)
        {
            var particle = _pool.Get();
            particle.transform.position = position;
            particle.transform.localScale = Vector3.one *_scale;
            particle.SetColor(color);
            particle.Play();
            soundPool.PlaySound();
            DOVirtual.DelayedCall(_returnDelay, () => particle.Pool.Return(particle)).SetUpdate(false);
        }
    }
}