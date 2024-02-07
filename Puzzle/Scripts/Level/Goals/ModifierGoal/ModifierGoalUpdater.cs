using DG.Tweening;
using GameCore.CrossScene.Scripts.Animations;
using GameCore.CrossScene.Scripts.Fx;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Field.Tiles.Modifiers;
using GameCore.Puzzle.Scripts.Level.Goals.Fx;
using GameCore.Puzzle.Scripts.Level.Goals.Views;
using JetBrains.Annotations;
using NaughtyAttributes;
using NepixSignals;
using StaserSDK;
using StaserSDK.Extentions;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Goals
{
    public class ModifierGoalUpdater : MonoBehaviour
    {
        [SerializeField] private MonoPool<ModifierGoalView> _goalViewPool;
        [SerializeField] private float _delay;
        [Space]
        [SerializeField] private float _startMoveDelay = 0.1f;

        [Space] [SerializeField] private TileModifierCollection _collection;
        [Inject, UsedImplicitly] public TileModifiersController tileModifiersController { get; }
        [Inject, UsedImplicitly] public GameBoard gameBoard { get; }
        [Inject, UsedImplicitly] public ModiferGoalDisplay modiferGoalDisplay { get; }
        [Inject, UsedImplicitly] public LevelController levelController { get; }
        [Inject, UsedImplicitly] public FlyAnimation flyAnimation { get; }
        [Inject, UsedImplicitly] public CompleteEffect completeEffect { get; }

        private float _currentDelay = 0.0f;
        private LevelModifierGoal _goal;
        private float moveDuration => flyAnimation.duration;

        public TheSignal started { get; } = new();
        public TheSignal startMove { get; } = new();
        public TheSignal ended { get; } = new();
        
        private void Awake()
        {
            _goalViewPool.Initialize();
        }
        
        private void OnEnable()
        {
            levelController.started.On(OnLeveStarted);
        }

        private void OnLeveStarted()
        {
            if(levelController.TryGetModifierGoal(out _goal) == false)
                return;
            
            tileModifiersController.claimed.On(OnModifierClaimed);
        }

        [Button()]
        public void Debug()
        {
            int x = Random.Range(0, gameBoard.size.x);
            int y = Random.Range(0, gameBoard.size.y);
            OnModifierClaimed(gameBoard.background[x, y].transform.position, _collection.modifiers.Random());
        }

        private void OnModifierClaimed(Vector3 position, TileModifier modifier)
        {
            started.Dispatch();
            var view = _goalViewPool.Get();
            view.Init(modifier);
            view.transform.position = position;

            if (modiferGoalDisplay.TryGetView(modifier, out var modifierView) == false)
                return;

            view.transform.position = position;
            flyAnimation.Kill(view.rectTransform);
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(_delay + _currentDelay);
            sequence.AppendCallback(startMove.Dispatch);
            sequence.Append(Move(view, modifierView.modifierImage.transform.position));
            sequence.AppendCallback(() =>
            {
                completeEffect.Play(view.transform.position, modifier.modifierColor);
                view.Pool.Return(view);
                _currentDelay -= _startMoveDelay;
                _goal.ActualizeGoal(modifier, 1);
                ended.Dispatch();
            });
            sequence.SetLink(gameObject);
            _currentDelay += _startMoveDelay;
        }

        private Tween Move(ModifierGoalView view, Vector3 destination)
        {
            return flyAnimation.MoveWithScaleAndRotation(view.rectTransform, destination, 2, 1f);
        }
    }
}