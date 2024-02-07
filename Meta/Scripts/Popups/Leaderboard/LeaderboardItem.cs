using JetBrains.Annotations;
using NepixCoreModules.NamesAndFlagsDB.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.Meta.Scripts.Popups.Leaderboard
{
    public class LeaderboardItem : MonoBehaviour, IPoolItem<LeaderboardItem>
    {
        [SerializeField] private Image _flag;
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private TMP_Text _playerRecord;
        [SerializeField] private LeaderboardPlace _leaderboardPlace;

        [Inject, UsedImplicitly] public IFlagSpriteProvider _flagSpriteProvider;
        [Inject, UsedImplicitly] public INameBotProvider _nameBotProvider;

        public Image flag => _flag;
        
        public IPool<LeaderboardItem> Pool { get; set; }
        public bool IsTook { get; set; }

        [ES3Serializable]
        public class SaveData
        {
            public int place;
            public string name;
            public int record;
            public int flagIndex;

            public SaveData() : this(-1, "", 0, 0){}
            
            public SaveData(int place, string name, int record, int flagIndex)
            {
                this.place = place;
                this.name = name;
                this.record = record;
                this.flagIndex = flagIndex;
            }
        }
        
        public void Initialize(int place, string playerName, int record, int flagIndex)
        {
            _playerName.text = playerName;
            _leaderboardPlace.SetPlace(place);
            _playerRecord.text = record.ToString();
            _flag.sprite = _flagSpriteProvider.Get(flagIndex);
        }
        
        public void Initialize(SaveData saveDataItem)
        {
            Initialize(saveDataItem.place, saveDataItem.name, saveDataItem.record, saveDataItem.flagIndex);
        }

        public SaveData InitializeRandom(int place, int record)
        {
            string playerName = _nameBotProvider.GetRandom();
            int flagIndex = _flagSpriteProvider.GetIndex(_flagSpriteProvider.GetRandom());
            var item = new SaveData(place, playerName, record, flagIndex);
            Initialize(item);
            return item;
        }
        
        public void TakeItem()
        {
        }

        public void ReturnItem()
        {
        }
    }
}