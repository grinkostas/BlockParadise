using System;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private float _delay;
        [SerializeField] private bool _preloadMeta;
        [InjectOptional, UsedImplicitly] public LevelController levelController { get; }
        [InjectOptional, UsedImplicitly] public LevelSceneSwitcher levelSceneSwitcher { get; }

        private void Start()
        {
            if(_preloadMeta)
                levelSceneSwitcher.PrepareMeta();
        }

        public void Retry()
        {
            if(levelController == null)
                return;
            levelController.Retry();
        }

        public void NextLevel()
        {
            if(levelController == null)
                return;
            levelController.NextLevel();
        }

        public void LoadMeta()
        {
            levelSceneSwitcher.PrepareMeta();
            DOVirtual.DelayedCall(_delay, ()=>levelSceneSwitcher.LoadMeta()).SetLink(gameObject);
        }
    }
}