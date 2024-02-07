using System;
using DG.Tweening;
using GameCore.Puzzle.Scripts.Level.Goals.Counters;
using GameCore.Puzzle.Scripts.Score;
using JetBrains.Annotations;
using StaserSDK.Extentions;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Classic.Views
{
    public abstract class BaseScoreView : MonoBehaviour
    {
        [SerializeField] protected ScoreCounter scoreCounter;
        [SerializeField] private float _setAmountDelay;

        [Inject, UsedImplicitly] public ScoreController scoreController { get; }

        private void OnEnable()
        {
            scoreCounter.ResetCounter();
            DOVirtual.DelayedCall(_setAmountDelay, SetCounter).ConfigureWithId(this, gameObject);
        }

        private void OnDisable()
        {
            DOTween.Kill(this);
        }

        protected abstract void SetCounter();
    }
}