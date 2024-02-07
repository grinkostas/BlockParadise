using DG.Tweening;
using NaughtyAttributes;
using NepixSignals;
using TMPro;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Score.Fx
{
    public class ScoreText : MonoBehaviour, IPoolItem<ScoreText>
    {
        [SerializeField] private GameObject _graphic;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private GameObject _descriptionContainer;
        [SerializeField] private float _showDescriptionDelay;
        [SerializeField] private TMP_Text _desctiptionText;
        public IPool<ScoreText> Pool { get; set; }
        public bool IsTook { get; set; }
        
        public void Show(int score, string description = "")
        {
            _scoreText.text = score.ToString();
            _desctiptionText.text = description;
            if (description != "")
                DOVirtual.DelayedCall(_showDescriptionDelay, () => _descriptionContainer.gameObject.SetActive(true)).SetLink(gameObject);

            _graphic.SetActive(true);
        }

        public void Hide()
        {
                
        }
        
        public void TakeItem()
        {
            _graphic.SetActive(false);
            _descriptionContainer.gameObject.SetActive(false);
        }

        public void ReturnItem()
        {
        }
    }
}