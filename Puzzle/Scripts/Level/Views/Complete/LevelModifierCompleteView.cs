using System;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.Scripts.Sounds.SoundPools;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Views
{
    public class LevelModifierCompleteView : MonoBehaviour
    {
        [SerializeField] private MonoPool<LevelModifierCompleteItem> _completeItemsPool;
        [SerializeField] private float _showItemDelay = 0.15f;
        [SerializeField] private bool _playParticle = true;
        
        [Inject, UsedImplicitly] public LevelController levelController { get; }
        [Inject, UsedImplicitly] public DiContainer container { get; }

        private List<LevelModifierCompleteItem> _spawnedItems = new();

        private void Awake()
        {
            _completeItemsPool.Initialize(container);
        }

        private void OnEnable()
        {
            foreach (var spawnedItem in _spawnedItems)
                spawnedItem.Pool.Return(spawnedItem);
            _spawnedItems.Clear();
            
            if (levelController == null || levelController.currentState == LevelController.LevelState.WaitForInit)  
                return;
            
            if (levelController.TryGetModifierGoal(out var goal) == false)
            {
                gameObject.SetActive(false);
                return;
            }

            float delay = _showItemDelay;
            foreach (var goalData in goal.goal)
            {
                DOVirtual.DelayedCall(delay, () =>
                {
                    var completeItem = _completeItemsPool.Get();
                    completeItem.modifierImage.sprite = goalData.modifier.sprite;
                    completeItem.Complete();
                    if(_playParticle)
                        completeItem.particle.Play();
                    _spawnedItems.Add(completeItem);
                }).SetLink(gameObject);
                delay += _showItemDelay;
            }
            
            
        }
    }
}