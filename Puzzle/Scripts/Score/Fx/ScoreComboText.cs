using TMPro;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Score.Fx
{
    public class ScoreComboText : MonoBehaviour , IPoolItem<ScoreComboText>
    {
        [SerializeField] private GameObject _graphic;
        [SerializeField] private TMP_Text _multiplayerText;
        public IPool<ScoreComboText> Pool { get; set; }
        public bool IsTook { get; set; }

        public void Show(int combo)
        {
            _multiplayerText.text = combo.ToString();
            _graphic.SetActive(true);
        }
        
        public void TakeItem()
        {
            _graphic.SetActive(false);
        }

        public void ReturnItem()
        {
        }
    }
}