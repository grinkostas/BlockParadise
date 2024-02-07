using DG.Tweening;
using JetBrains.Annotations;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Views
{
    public class LevelScoreCompleteView : MonoBehaviour
    {
        [SerializeField] private RectTransform _scoreRectTransform;
        [SerializeField] private TMP_Text _scoreGoalText;
        [SerializeField] private float _textAnimationDuration;
        [SerializeField] private bool _destroyAndPlayParticle = true;
        [SerializeField, ShowIf(nameof(_destroyAndPlayParticle))] private ParticleSystem _particleSystem;
        [SerializeField, ShowIf(nameof(_destroyAndPlayParticle))] private float _particlePlayDelay;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField, ShowIf(nameof(_destroyAndPlayParticle))] private AudioSource _particleSound;

        [Inject, UsedImplicitly] public LevelController levelController { get; }
        
        [Button()]
        private void OnEnable()
        {
            if(levelController == null || levelController.currentState == LevelController.LevelState.WaitForInit)
                return;
            
            if (levelController.TryGetScoreGoal(out var goal) == false)
            {
                gameObject.SetActive(false);
                return;
            }
            _scoreGoalText.text = "0";
            _audioSource.Play();
            DOVirtual.Float(0, goal.target, _textAnimationDuration,
                value => _scoreGoalText.text = $"{Mathf.RoundToInt(value)}");
            if(_destroyAndPlayParticle == false)
                return;
            DOVirtual.DelayedCall(_particlePlayDelay, () =>
            {
                _particleSound.Play();
                _particleSystem.Play();
                _scoreRectTransform.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }
    }
}