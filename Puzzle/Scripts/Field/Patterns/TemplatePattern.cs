using UnityEngine;

namespace GameCore.Puzzle.Scripts.Field.Patterns
{
    public class TemplatePattern : TilesPattern
    {
        [SerializeField] private int _maxInRow = 1;
        [SerializeField] private int _complexity = 1;
        [SerializeField] private bool _rotable = true;
        
        public int maxInUnit => _maxInRow;
        public int complexity => _complexity;
        public bool rotatable => _rotable;
    }
}