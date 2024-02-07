using GameCore.Puzzle.Scripts.Field.Patterns;
using NaughtyAttributes;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Field.Figures
{
    public class FigureDebuger : MonoBehaviour
    {
        [SerializeField] private Figure _figure;
        [SerializeField] private int _rotation;
        [SerializeField] private FigurePattern _figurePattern;
        
        [Button()]
        private void Init()
        {
            _figure.InitFigure(_figurePattern.GetPattern(), _rotation);
        }

        private void Reset()
        {
            _figure.ResetFigure();
        }
    }
}