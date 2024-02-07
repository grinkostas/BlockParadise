using System;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Animations;
using GameCore.Puzzle.Scripts.Level.Goals.Fx;
using JetBrains.Annotations;
using StaserSDK.Extentions;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Goals.Views
{
    public class ModifierGoalEndGameDisplay : MonoBehaviour
    {
        [SerializeField] private MonoPool<ModiferGoalDisplayView> _viewsPool;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _showDelay;
        [SerializeField] private float _shotItemDelay;
        [SerializeField] private float _completeDelay;

        [Inject, UsedImplicitly] public DiContainer container { get; }
        [Inject, UsedImplicitly] public LevelController levelController { get; }

        private LevelModifierGoal _goal;
        private List<ModiferGoalDisplayView> _spawnedViews = new();

        private void Awake()
        {
            _viewsPool.Initialize(container);
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            ClearSpawnedViews();
            if (levelController.TryGetModifierGoal(out _goal) == false)
            {
                _canvasGroup.alpha = 0.0f;
                return;
            }
            _canvasGroup.alpha = 1.0f;
            ShowViews();
        }

        private void ClearSpawnedViews()
        {
            foreach (var spawnedView in _spawnedViews)
                spawnedView.Pool.Return(spawnedView);
            _spawnedViews.Clear();
        }

        private void ShowViews()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(_showDelay);
            for (int i = 0; i < _goal.goal.Count; i++)
            {
                int index = i;
                sequence.AppendCallback(() => InitView(SpawnView(), index));
                sequence.AppendInterval(_shotItemDelay);
            }

            sequence.ConfigureWithId(this, gameObject);
        }

        private ModiferGoalDisplayView SpawnView()
        {
            var view = _viewsPool.Get();
            _spawnedViews.Add(view);
            view.transform.localScale = Vector3.zero;
            _spawnedViews.Add(view);
            view.transform.ZoomIn();
            return view;
        }

        private void InitView(ModiferGoalDisplayView view, int goalIndex)
        {
            var goalData = _goal.goal[goalIndex];
            var completeData = _goal.goalCompleteData[goalIndex];
            
            view.Init(_goal, goalData, completeData);
            view.goalText.gameObject.SetActive(true);
            
            if(goalData.amount - completeData.amount > 0) return;
            view.goalText.gameObject.SetActive(false);
            DOVirtual.DelayedCall(_completeDelay, view.Complete).ConfigureWithId(this, gameObject);
        }
    }
}