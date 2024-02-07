using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Meta.Scripts.Popups.Leaderboard
{
    public class LeaderboardPlace : MonoBehaviour
    {
        [Header("Default Place")]
        [SerializeField] private GameObject _defaultPlace;
        [SerializeField] private TMP_Text _defaultPlaceText;
        [Header("TopPlace")] 
        [SerializeField] private List<TopPlaceData> _topPlacesData;
        [SerializeField] private GameObject _topPlace;
        [SerializeField] private Image _medalImage;
        [SerializeField] private TMP_Text _topPlaceText;
        
        [System.Serializable]
        public class TopPlaceData
        {
            public int place;
            public Sprite medal;
        }

        public void SetPlace(int place)
        {
            int maxMedalPlace = _topPlacesData.Max(x => x.place);
            if(place <= maxMedalPlace)
                SetMedalPlace(place);
            else
                SetDefaultPlace(place);
        }

        private void SetMedalPlace(int place)
        {
            _topPlace.SetActive(true);
            _defaultPlace.SetActive(false);
            _topPlaceText.text = place.ToString();
            _medalImage.sprite = _topPlacesData.Find(x => x.place == place).medal;
        }

        private void SetDefaultPlace(int place)
        {
            _topPlace.SetActive(false);
            _defaultPlace.SetActive(true);
            _defaultPlaceText.text = place.ToString();
        }
    }
}