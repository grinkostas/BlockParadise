using DG.Tweening;
using GameCore.Puzzle.Scripts.Level;
using JetBrains.Annotations;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Figures
{
    public class FigureRemover : LevelListener
    {
        [InjectOptional, UsedImplicitly] public FigureSpawner figureSpawner { get; }

        protected override void OnLevelEnded()
        {
            if(figureSpawner == null)
                return;
            foreach (var figure in figureSpawner.spawnedFigures)
            {
                figure.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetId(this);
            }
        }

    }
}