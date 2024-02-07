using System;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Sounds;
using GameCore.Scripts.Sounds;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Level.Goals
{
    public class ModifierGoalSounds : MonoBehaviour
    {
        [SerializeField] private ModifierGoalUpdater _modifierGoalUpdater;
        [SerializeField] private SoundPool _showSound;
        [SerializeField] private SoundPool _flySound;
        [SerializeField] private SoundPool _claimSound;
        [SerializeField] private float _flySoundPlayDuration;

        private void OnEnable()
        {
            _modifierGoalUpdater.started.On(OnStarted);
            _modifierGoalUpdater.startMove.On(OnStartMove);
            _modifierGoalUpdater.ended.On(OnEnded);
        }

        private void OnDisable()
        {
            _modifierGoalUpdater.started.Off(OnStarted);
            _modifierGoalUpdater.startMove.Off(OnStartMove);
            _modifierGoalUpdater.ended.Off(OnEnded);
        }

        private void OnStarted()
        {
            _showSound.PlaySound();
        }

        private void OnStartMove()
        {
            DOVirtual.DelayedCall(_flySoundPlayDuration, _flySound.PlaySound).SetLink(gameObject);
        }

        private void OnEnded()
        {
            _claimSound.PlaySound();
        }
    }
}