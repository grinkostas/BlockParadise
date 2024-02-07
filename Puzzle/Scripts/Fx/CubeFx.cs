using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Puzzle.Scripts.Fx
{
    public class CubeFx : MonoBehaviour, IPoolItem<CubeFx>
    {
        [SerializeField] private Image _cubeImage;
        [SerializeField] private CanvasGroup _canvasGroup;

        public CanvasGroup canvasGroup => _canvasGroup;
        public IPool<CubeFx> Pool { get; set; }
        public bool IsTook { get; set; }

        public void SetColor(Color color)
        {
            _cubeImage.color = color;
        }
        
        public void TakeItem()
        {
        }

        public void ReturnItem()
        {
        }
    }
}