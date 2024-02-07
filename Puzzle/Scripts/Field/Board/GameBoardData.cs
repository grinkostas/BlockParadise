using System.Collections.Generic;
using GameCore.Puzzle.Scripts.Field.Colors;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Field.Board
{
    public class GameBoardData : MonoBehaviour
    {
        [SerializeField] private List<ColorTemplate> _tileColorTemplates;

        public List<ColorTemplate> tileColorTemplates => _tileColorTemplates;

        public ColorTemplate GetRandomColor()
        {
            return _tileColorTemplates.Random();
        }
    }
}