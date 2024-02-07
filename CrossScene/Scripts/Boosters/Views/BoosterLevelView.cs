using System;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Animations;
using GameCore.CrossScene.Scripts.Boosters.Core;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameCore.CrossScene.Scripts.Boosters.Views
{
    public class BoosterLevelView : MonoBehaviour
    {
        [SerializeField] private Booster _booster;
        [SerializeField] private TMP_Text _countText;
        
        [Inject, UsedImplicitly] public BoostersController boostersController { get; }

        private void OnEnable()
        {
            Actualize();
            boostersController.usedBoosted.On(OnUsedBooster);
        }

        private void OnDisable()
        {
            boostersController.usedBoosted.Off(OnUsedBooster);
        }

        private void OnUsedBooster(string id)
        {
            if(id != _booster.id) return;
            Actualize();
        }

        private void Actualize()
        {
            int count = boostersController.GetCount(_booster.id);
            if (count == 0)
            {
                Hide();
                return;
            }
            
            gameObject.transform.localScale = Vector3.one;
            gameObject.SetActive(true);

            _countText.text = count.ToString();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}