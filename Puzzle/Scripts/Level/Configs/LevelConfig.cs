using System.Collections.Generic;
using GameCore.Puzzle.Scripts.Field.Patterns;
using NaughtyAttributes;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Level.Configs
{
    [CreateAssetMenu(menuName = "Level/Config")]
    public class LevelConfig : ScriptableObject
    {
        public TemplatePattern levelPattern;
        public bool overrideSpawn = true;
        [ShowIf(nameof(overrideSpawn))]public List<SpawnVariation> spawnVariations;
    }
    
    [System.Serializable]
    public class SpawnVariation
    {
        public FigureSpawnData figure1;
        public FigureSpawnData figure2;
        public FigureSpawnData figure3;

        public FigureSpawnData[] figures => new []{figure1, figure2, figure3};
    }
        
    [System.Serializable]
    public class FigureSpawnData
    {
        public FigurePattern figure;
        public int rotation;
    }
}