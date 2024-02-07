using System;
using DG.Tweening;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Level.Views
{
    public class LevelStartView : LevelListener
    {
        [SerializeField] private AlphaView _alphaView;
        [SerializeField] private float _showDelay;
        [SerializeField] private float _hideDelay;

        protected override void OnLevelStarted()
        {
            DOVirtual.DelayedCall(_showDelay, _alphaView.Show).SetUpdate(false).SetLink(gameObject);
            DOVirtual.DelayedCall(_showDelay+_hideDelay, _alphaView.Hide).SetLink(gameObject).SetUpdate(false);
        }
    }
}