using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Meta.Scripts.Popups.NextLevel
{
    public class LevelModifierView : MonoBehaviour, IPoolItem<LevelModifierView>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _amountText;
        public IPool<LevelModifierView> Pool { get; set; }
        public bool IsTook { get; set; }

        public void Initialize(Sprite icon, int amount)
        {
            _icon.sprite = icon;
            _amountText.text = amount.ToString();
        }
        
        public void TakeItem()
        {
        }

        public void ReturnItem()
        {
        }
    }
}