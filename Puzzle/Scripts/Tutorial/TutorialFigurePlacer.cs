using GameCore.Puzzle.Scripts.Field;
using GameCore.Puzzle.Scripts.Field.Figures;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Tutorial
{
    public class TutorialFigurePlacer : FigurePlacer
    {
        [Inject, UsedImplicitly] public MatchChecker matchChecker { get; }
        
        protected override void PlaceFigure(Vector2Int[] coords, Figure figure)
        {
            matchChecker.GetCompletedLines(out int lineCount, coords);
            if (lineCount < 2)
            {
                returned.Dispatch();
                return;
            }

            base.PlaceFigure(coords, figure);
        }
    }
}