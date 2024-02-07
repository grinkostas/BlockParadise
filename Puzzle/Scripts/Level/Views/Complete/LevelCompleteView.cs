using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Views
{
    public class LevelCompleteView : LevelListener
    {
        [SerializeField] private View _view;
        [SerializeField] private float _view2ShowDelay;
        [SerializeField] private View _view2;
        [SerializeField] private AudioSource _audioSource;
        
        [Inject, UsedImplicitly] public LevelSceneSwitcher sceneSwitcher { get; }
        
        protected override void OnLevelCompleted()
        {
            _audioSource.Play();
            sceneSwitcher.PrepareMeta();
            _view.Show();
            DOVirtual.DelayedCall(_view2ShowDelay, _view2.Show).SetLink(gameObject);
        }
    }
}