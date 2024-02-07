using System.Collections.Generic;
using System.Linq;
using GameCore.CrossScene.Scripts.UI.Popups;
using GameCore.Puzzle.Scripts.Classic.Core;
using JetBrains.Annotations;
using NepixCoreModules.NamesAndFlagsDB.Scripts;
using UnityEngine;
using Zenject;

namespace GameCore.Meta.Scripts.Popups.Leaderboard
{
    public class LeaderboardPopup : NamedPopup
    {
        [SerializeField] private MonoPool<LeaderboardItem> _itemsPool;
        [SerializeField] private int _places = 25;
        [SerializeField] private int _maxScore = 9999;
        [SerializeField] private int _minScore = 2500;
        [SerializeField] private LeaderboardItem _playerItem;
        [SerializeField] private Sprite _playerIcon;

        [Inject, UsedImplicitly] public DiContainer container { get; }
        [Inject, UsedImplicitly] public IFlagSpriteProvider _flagSpriteProvider;
        
        private List<LeaderboardItem> _spawnedViews = new();
        private const string _saveKey = "FakeLeaderBoardSave";

        [Inject]
        protected override void OnInject()
        {
            base.OnInject();
            _itemsPool.Initialize(container);
        }
        
        protected override void OnShowStart()
        {
            ClearViews();
            int playerPlace = InitPlayerLeaderboardItem();
            List<LeaderboardItem.SaveData> saves = new();
            if (ES3.KeyExists(_saveKey))
            {
                saves = ES3.Load<List<LeaderboardItem.SaveData>>(_saveKey).OrderBy(x=>x.place).ToList();
                int index = 0;
                foreach (var save in saves)
                {
                    index++;
                    if(index > _places)
                        break;
                    if(save.place == playerPlace)
                        continue;
                    var view = _itemsPool.Get();
                    view.Initialize(save);
                    view.transform.SetSiblingIndex(save.place);
                    _spawnedViews.Add(view);
                }
                return;
            }
            
            int step = (_maxScore - _minScore) / _places;
            int currentScore = _maxScore;
            for (int i = 1; i < _places + 1; i++)
            {
                if(playerPlace == i)
                    continue;
                int score = Random.Range(currentScore - step/2, currentScore + step/2);
                currentScore -= step;
                var view = _itemsPool.Get();
                var leaderboardItemSave = view.InitializeRandom(i, score);
                saves.Add(leaderboardItemSave);
                _spawnedViews.Add(view);
            }
            ES3.Save(_saveKey, saves);
        }

        private void ClearViews()
        {
            foreach (var view in _spawnedViews)
                view.Pool.Return(view);
            
            _spawnedViews.Clear();
        }

        private int InitPlayerLeaderboardItem()
        {
            int step = (_maxScore - _minScore) / _places;
            int playerRecord = ClassicLevelController.playerMaxScore;
            int place = (_places + playerRecord / step) + ((_minScore - playerRecord)/500) * 300;
            if (playerRecord == 0)
                place = 9999;
            else if (playerRecord >= _maxScore)
                place = 1;
            else if (playerRecord >= _minScore)
                place = (_maxScore/step) - ((_maxScore - playerRecord ) / step);

            int flagIndex = _flagSpriteProvider.GetIndex(_flagSpriteProvider.GetForCurrentCulture());
            _playerItem.Initialize(place, "You", playerRecord, flagIndex);
            _playerItem.flag.sprite = _playerIcon;
            return place;
        }
        
    }
}