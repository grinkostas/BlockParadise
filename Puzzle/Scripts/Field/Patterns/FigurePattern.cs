using UnityEngine;

namespace GameCore.Puzzle.Scripts.Field.Patterns
{
    public enum FigureComplexity
    {
        Small, 
        Medium, 
        Large
    }
    public class FigurePattern : TilesPattern
    {
        [SerializeField] private FigureComplexity _complexity;

        public FigureComplexity complexity => _complexity;
    }
}