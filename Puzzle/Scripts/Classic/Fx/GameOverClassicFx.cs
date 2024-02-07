using DG.Tweening;
using GameCore.Puzzle.Scripts.Classic.Core;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Level;
using GameCore.Puzzle.Scripts.Score;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Classic.Fx
{
    public class GameOverClassicFx : LevelListener
    {
        [SerializeField] private AlphaView _gameOverView;
        [SerializeField] private AlphaView _gameOverPopup;
        [SerializeField] private AlphaView _bestScorePopup;
        [SerializeField] private float _startFillDelay;
        [SerializeField] private float _showPopupDelay;
        [Inject, UsedImplicitly] public GameBoardUtilities boardUtilities { get; }
        [Inject, UsedImplicitly] public LevelSceneSwitcher sceneSwitcher { get; }
        [Inject, UsedImplicitly] public ClassicLevelController classicLevelController { get; }
        [Inject, UsedImplicitly] public ScoreController scoreController { get; }
        
        [Button()]
        protected override void OnLevelEnded()
        {
            DOVirtual.DelayedCall(_startFillDelay, boardUtilities.FillBoardRandomColors).SetId(this).SetLink(gameObject);
        }

        protected override void OnLevelFailed()
        {
            _gameOverView.Show();
            
            DOVirtual.DelayedCall(_showPopupDelay, ()=>
            {
                if (scoreController.amount <= classicLevelController.maxScore)
                    _gameOverPopup.Show();
                else
                    _bestScorePopup.Show();
            }).SetId(this).SetLink(gameObject);
        }
    }
}