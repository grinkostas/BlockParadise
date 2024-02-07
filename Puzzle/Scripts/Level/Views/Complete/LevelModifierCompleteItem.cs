using DG.Tweening;
using GameCore.CrossScene.Scripts.Fx;
using GameCore.CrossScene.Scripts.Sounds.SoundPools;
using GameCore.Scripts.Sounds.SoundPools;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Views
{
    public class LevelModifierCompleteItem : MonoBehaviour, IPoolItem<LevelModifierCompleteItem>
    {
        [SerializeField] private Image _modifierImage;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private GameObject _checkBox;
        [SerializeField] private float _checkBoxShowDelay;

        [Inject, UsedImplicitly] public CompleteEffect completeEffect { get; }
        
        public Image modifierImage => _modifierImage;
        public ParticleSystem particle => _particle;
        
        public IPool<LevelModifierCompleteItem> Pool { get; set; }
        public bool IsTook { get; set; }

        public void Complete()
        {
            DOVirtual.DelayedCall(_checkBoxShowDelay, () =>
            {
                _checkBox.SetActive(true);
                completeEffect.Play(_checkBox.transform.position);
            }).SetId(this).SetLink(gameObject);
        }
        
        public void TakeItem()
        {
            _checkBox.SetActive(false);
        }

        public void ReturnItem()
        {
            DOTween.Kill(this);
        }
    }
}