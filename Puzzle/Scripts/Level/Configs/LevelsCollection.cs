using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Level.Configs
{
    [CreateAssetMenu(menuName = "Level/Collection")]
    public class LevelsCollection : ScriptableObject
    {
        public List<LevelConfig> levels;
    }
}