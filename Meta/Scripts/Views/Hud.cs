using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameCore.Meta.Scripts.Views
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private List<CanvasGroup> _hudItems;

        public void Show()
        {
            DOTween.Kill(this);
            foreach (var item in _hudItems)
                item.DOFade(1, 0.35f).SetId(this);
        }

        public void Hide()
        {
            DOTween.Kill(this);
            foreach (var item in _hudItems)
                item.DOFade(0, 0.35f).SetId(this);
        }
    }
}