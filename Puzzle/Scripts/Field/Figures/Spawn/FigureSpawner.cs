using System.Collections.Generic;
using System.Linq;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Field.Patterns;
using GameCore.Puzzle.Scripts.Level;
using JetBrains.Annotations;
using NaughtyAttributes;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Figures
{
    public class FigureSpawner : MonoBehaviour
    {
        [SerializeField] private Figure _figurePrefab;
        [SerializeField] private FigureCollection _figureCollection;
        [SerializeField] private RectTransform _figuresParent;
        [Space] 
        [SerializeField] private List<SpawnData> _spawnData;
        
        [Inject, UsedImplicitly] public GameBoard board { get; }
        [Inject, UsedImplicitly] public GameBoardData boardData { get; }
        [Inject, UsedImplicitly] public LevelController levelController { get; }
        [Inject, UsedImplicitly] public DiContainer container { get; }

        [System.Serializable]
        public class SpawnData
        {
            public bool large;
            public bool medium;
            public bool small;
            public int maxFreeTiles;
        }
        
        private List<SpawnData> _sortedSpawnData = new();
        
        public TheSignal<Figure> spawned { get; } = new();
        public TheSignal spawnedVariations { get; } = new();

        public List<Figure> spawnedFigures  = new();

        private int _freePlaces = 64;
        private int _spawedVariations = 0;

        [Button()]
        public Figure Spawn(int freePlace = 64, bool first = false)
        {
            var figure = GetFigure(freePlace);
            int rotation = Random.Range(0, 4) * 90;
            return Spawn(figure, rotation);
        }
        
        private int[,] GetFigure(int freePlace = 64, bool first = false)
        {
            var spawnData = GetSpawnData(freePlace);
            List<int[,]> variations = GetVariations(spawnData);
            if (first)
            {
                variations = variations.FindAll(x => board.CanPlaceFigureOnBoard(x));
            }
            var figure = variations.Random();
            return figure;
        }
        
        private SpawnData GetSpawnData(int freePlace = 64)
        {
            if(_sortedSpawnData.Count == 0)
                _sortedSpawnData = _spawnData.OrderByDescending(x => x.maxFreeTiles).ToList();
            return _sortedSpawnData.Find(x => freePlace >= x.maxFreeTiles);
        }

        public Figure Spawn(int[,] figureCords, int rotation)
        {
            Figure figure = container.InstantiatePrefab(_figurePrefab, _figuresParent).GetComponent<Figure>();
            figure.InitFigure(figureCords, rotation);
            
            var color = boardData.GetRandomColor();
            figure.SetColor(color);
            
            spawnedFigures.Add(figure);
            spawned.Dispatch(figure);
            return figure;
        }
        
        public void SpawnVariation(int variationsCount, bool first = false)
        {
            _freePlaces = board.GetEmptyTilesCount();
            for (int i = 0; i < variationsCount; i++)
            {
                var figure = Spawn(_freePlaces, first);
                _freePlaces -= figure.size;
            }
            spawnedVariations.Dispatch();
        }
        
        private List<int[,]> GetVariations(SpawnData spawnData)
        {
            List<int[,]> variations = new();
            if(spawnData.large)
                variations.AddRange(_figureCollection.GetRange(FigureComplexity.Large));
            if(spawnData.medium)
                variations.AddRange(_figureCollection.GetRange(FigureComplexity.Medium));
            if(spawnData.small)
                variations.AddRange(_figureCollection.GetRange(FigureComplexity.Small));
            return variations;
        }


        public void Remove(Figure figure)
        {
            spawnedFigures.Remove(figure);
        }
    }
}