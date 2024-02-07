using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Puzzle.Scripts.Field.Tiles.Modifiers;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Goals
{
    public class LevelModifierGoal : LevelGoal
    {
        public override GoalType type => GoalType.Modifier;
        
        [System.Serializable]
        public class GoalData
        {
            public TileModifier modifier;
            public int amount;
            public bool completed { get; set; } = false;

            public GoalData() : this(null, 0){}
            
            public GoalData(TileModifier modifier, int amount)
            {
                this.modifier = modifier;
                this.amount = amount;
            }

            public GoalData(GoalData data)
            {
                modifier = data.modifier;
                amount = data.amount;
            }
        }

        [System.Serializable]
        public class GoalSaveData
        {
            public string modifierId;
            public int amount;

            public GoalSaveData() : this("", 0)
            {
            }

            public GoalSaveData(string modifierId, int amount)
            {
                this.modifierId = modifierId;
                this.amount = amount;
            }
        }

        public List<GoalData> goal { get; } = new();
        public List<GoalData> goalCompleteData { get; } = new();

        public TheSignal<TileModifier> goalProgressChanged { get; } = new();

        public void SetGoal(List<GoalData> targetGoal)
        {
            goal.AddRange(targetGoal); 
            foreach (var goalData in targetGoal)
                goalCompleteData.Add(new GoalData(goalData.modifier, 0));
        }

        public int GetGoalReminder(TileModifier modifier)
        {
            var targetGoal = goal.Find(x => x.modifier.id == modifier.id);
            var completeData = goalCompleteData.Find(x => x.modifier.id == modifier.id);
            if (completeData.amount >= targetGoal.amount)
                return 0;
            return targetGoal.amount - completeData.amount;
        }

        public void ActualizeGoal(TileModifier modifier, int amount = 1)
        {
            var goalData =  goalCompleteData.Find(x => x.modifier.id == modifier.id);
            
            if(goalData == null)
                return;
            int reminder = GetGoalReminder(modifier);
            if (reminder <= 0)
            {
                Complete(goalData);
                return;
            }
            
            goalData.amount += Math.Min(amount, reminder);

            goalProgressChanged.Dispatch(modifier);
            TryComplete(goalData);
        }

        private void TryComplete(GoalData goalData)
        {
            if(GetGoalReminder(goalData.modifier) > 0)
                return;
            Complete(goalData);
        }
        
        private void Complete(GoalData goalData)
        {
            if (goalData.completed == false)
                goalData.completed = true;
            if(goalCompleteData.Has(x=>x.completed == false))
                return;
            Complete();
        }

        public override float GetProgress()
        {
            return (float)goal.Sum(x=>x.amount) / goalCompleteData.Sum(x=>x.amount);
        }
    }
}