using System.Collections.Generic;
using DG.Tweening;
using GameCore.Puzzle.Scripts.Level;
using GameCore.Puzzle.Scripts.Level.Configs;
using JetBrains.Annotations;
using NUnit.Framework;
using StaserSDK.Extentions;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Figures
{
    public class FigureSpawnInvoker : LevelListener
    {
        [SerializeField] private float _levelStartSpawnDelay;
        [SerializeField] private int _variationsCount;
        [SerializeField] private List<RectTransform> _figureSpawnPoints;
        [SerializeField] private FigureSpawner _figureSpawner;
        
        [Inject, UsedImplicitly] public FigurePlacer figurePlacer { get; }

        private List<SpawnVariation> _variationPresets = new();
        
        protected override void OnEnable()
        {
            base.OnEnable();
            levelController.onLevelPatternInitialize.On(OnLevelPatternInitialize);
            _figureSpawner.spawned.On(OnFigureSpawned);
            figurePlacer.placed.On(OnFigurePlaced);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _figureSpawner.spawned.Off(OnFigureSpawned);
            figurePlacer.placed.Off(OnFigurePlaced);
        }

        protected override void OnLevelStarted()
        {
            DOVirtual.DelayedCall(_levelStartSpawnDelay, () =>
            {
                if(TrySpawnVariationPreset() == false)
                    _figureSpawner.SpawnVariation(_variationsCount, true);
            })
            .ConfigureWithId(this, gameObject);
        }

        private bool TrySpawnVariationPreset()
        {
            if (_variationPresets.Count <= 0)
                return false;
            
            var variation = _variationPresets[0];
            _variationPresets.Remove(variation);
            foreach (var spawnData in variation.figures)
            {
                _figureSpawner.Spawn(spawnData.figure.GetPattern(), spawnData.rotation);
            }
            
            return true;
        }

        protected override void OnLevelEnded()
        {
            DOTween.Kill(this);
            _figureSpawner.spawned.Off(OnFigureSpawned);
            figurePlacer.placed.Off(OnFigurePlaced);
        }

        private void OnLevelPatternInitialize(LevelConfig levelConfig)
        {
            _variationPresets.AddRange(levelConfig.spawnVariations);
        }
        
        private void OnFigureSpawned(Figure figure)
        {
            var spawnPoint = _figureSpawnPoints[Mathf.Min(_figureSpawner.spawnedFigures.Count-1, _figureSpawnPoints.Count - 1)];
            figure.transform.position = spawnPoint.position;
        }

        private void OnFigurePlaced(Figure figure)
        {
            _figureSpawner.Remove(figure);
            if (_figureSpawner.spawnedFigures.Count == 0)
            {
                if(TrySpawnVariationPreset() == false)
                    _figureSpawner.SpawnVariation(_variationsCount);
            }
        }

    }
}