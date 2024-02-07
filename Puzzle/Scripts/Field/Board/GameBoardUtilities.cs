using DG.Tweening;
using GameCore.CrossScene.Scripts.Sounds;
using GameCore.Scripts.Sounds;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Board
{
    public class GameBoardUtilities : MonoBehaviour
    {
        [SerializeField] private float _fillTileDelay;
        [SerializeField] private float _tileFadeDuration;
        [SerializeField] private SoundPool _fillLineSound;
        [Inject, UsedImplicitly] public GameBoard board { get; }
        [Inject, UsedImplicitly] public GameBoardData boardData { get; }
        
        public void FillBoardRandomColors()
        {
            DOTween.Kill(this);
            int coloredTiles = 0;
            _fillLineSound.PlaySound();
            for (int y = 0; y < board.size.y; y++)
            {
                for(int x = 0; x < board.size.x; x++)
                {
                    int xCached = x;
                    int yCached = y;
                    DOVirtual.DelayedCall(_fillTileDelay * coloredTiles, 
                        ()=>
                        {
                            SetRandomColor(xCached, yCached);
                        }).SetId(this).SetLink(gameObject).SetUpdate(false);
                }
                coloredTiles++;
            }
        }
        [Button()]
        private void SetRandomColor(int x = 0 , int y = 0)
        {
            board.background[x, y].SetColor(boardData.GetRandomColor());
            board.background[x, y].canvasGroup.alpha = 0;
            board.background[x, y].canvasGroup.DOFade(1, _tileFadeDuration).SetLink(gameObject);

        }
        
        public void ClearBackgroundColors()
        {
            DOTween.Kill(this);
            int coloredTiles = 0;
            for (int y = board.size.y-1; y >= 0; y--)
            {
                for(int x = 0; x < board.size.x; x++)
                {
                    int xCached = x;
                    int yCached = y;
                    DOVirtual.DelayedCall(_fillTileDelay * coloredTiles, 
                        ()=> SetDefaultColor(xCached, yCached)).SetId(this).SetLink(gameObject).SetUpdate(false);
                }

                coloredTiles++;
            }
        }

        [Button()]
        private void SetDefaultColor(int x = 0, int y = 0)
        {
            var tile = board.background[x, y];
            board.background[x, y].canvasGroup.DOFade(0, _tileFadeDuration).SetLink(gameObject)
                .OnComplete(() => tile.SetColor(tile.defaultColor));
        }

    }
}