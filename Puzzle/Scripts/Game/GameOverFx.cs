using DG.Tweening;
using GameCore.CrossScene.Scripts.UI.Popups;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Field.Figures;
using GameCore.Puzzle.Scripts.Level;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Game
{
    public class GameOverFx : LevelListener
    {
        [SerializeField] private AlphaView _gameOverView;
        [SerializeField] private float _startFillDelay;
        [SerializeField] private float _showPopupDelay;
        [Inject, UsedImplicitly] public GameBoardUtilities boardUtilities { get; }
        [Inject, UsedImplicitly] public PopupManager popupManager { get; }
        [Inject, UsedImplicitly] public LevelSceneSwitcher sceneSwitcher { get; }

        [Button()]
        protected override void OnLevelEnded()
        {
            DOVirtual.DelayedCall(_startFillDelay, boardUtilities.FillBoardRandomColors).SetId(this).SetLink(gameObject);
        }

        protected override void OnLevelFailed()
        {
            sceneSwitcher.PrepareLevel();
            _gameOverView.Show();
            DOVirtual.DelayedCall(_showPopupDelay, ()=>
            {
                if(popupManager.TryGet<GameOverPopup>(out var gameOverPopup))
                    gameOverPopup.Show();
            }).SetId(this).SetLink(gameObject);
        }

    }
}