using System.Collections.Generic;
using System.Linq;
using GameCore.Puzzle.Scripts.Field.Patterns;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Field.Figures
{
    [CreateAssetMenu(menuName = "Collections/Figures")]
    public class FigureCollection : ScriptableObject
    {
        public List<FigurePattern> figures;

        public IEnumerable<int[,]> GetRange(FigureComplexity figureComplexity)
        {
            return figures.Where(x => x.complexity == figureComplexity).Select(x=>x.GetPattern());
        }
    }
}