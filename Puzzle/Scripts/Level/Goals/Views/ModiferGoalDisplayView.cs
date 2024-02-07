using DG.Tweening;
using GameCore.CrossScene.Scripts.Fx;
using GameCore.Puzzle.Scripts.Field.Tiles.Modifiers;
using JetBrains.Annotations;
using StaserSDK.Extentions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Goals.Views
{
    public class ModiferGoalDisplayView : MonoBehaviour, IPoolItem<ModiferGoalDisplayView>
    {
        [SerializeField] private Image _modifierImage;
        [SerializeField] private TMP_Text _goalText;
        [SerializeField] private Bouncer _bouncer;
        [Header("Complete")] 
        [SerializeField] private Image _checkMark;
        [SerializeField] private float _zoomOutTime;
        [SerializeField] private float _zoomInTime;
        
        [Inject, UsedImplicitly] public CompleteEffect completeEffect { get; }
        
        private LevelModifierGoal _goal;

        private bool _completed = false;
        public Image modifierImage => _modifierImage;
        public TMP_Text goalText => _goalText;
        public TileModifier modifier { get; private set; }
        public IPool<ModiferGoalDisplayView> Pool { get; set; }
        public bool IsTook { get; set; }

        public void Init(LevelModifierGoal goal, LevelModifierGoal.GoalData goalData, LevelModifierGoal.GoalData completedGoalData)
        {
            _goal = goal;
            modifier = goalData.modifier;
            
            _modifierImage.sprite = goalData.modifier.sprite;
            int amount = goalData.amount - completedGoalData.amount;
            _goalText.text = amount.ToString();
            _goal.goalProgressChanged.On(ActualizeGoalText);
        }
        
        
        private void ActualizeGoalText(TileModifier tileModifier)
        {
            if(tileModifier.id != modifier.id)
                return;

            if (_completed == false)
            {
                int reminder = _goal.GetGoalReminder(tileModifier);
                if (reminder <= 0)
                    Complete();
            
                _goalText.text = reminder.ToString();
            }
            
            _bouncer.Bounce();
        }

        public void Complete()
        {
            completeEffect.Play(_checkMark.transform.position);
            _completed = true;
            _goalText.transform.DOScale(0, _zoomOutTime).SetEase(Ease.InBack).SetLink(gameObject);
            _checkMark.transform.localScale = Vector3.zero;
            _checkMark.gameObject.SetActive(true);
            _checkMark.transform.DOScale(1, _zoomInTime).SetEase(Ease.OutBack).SetLink(gameObject);
        }
        
        public void TakeItem()
        {
            _completed = false;
            _checkMark.gameObject.SetActive(false);
            _goalText.gameObject.SetActive(true);
            _goalText.transform.localScale = Vector3.one;
        }

        public void ReturnItem()
        {
            modifier = null;
            if(_goal == null)
                return;
            _goal.goalProgressChanged.Off(ActualizeGoalText);
            _goal = null;
        }
    }
}