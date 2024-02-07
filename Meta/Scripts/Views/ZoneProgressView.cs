using System;
using GameCore.Meta.Scripts.Zone;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameCore.Meta.Scripts.Views
{
    public class ZoneProgressView : MonoBehaviour
    {
        [SerializeField] private SimpleSlider _progressSlider;
        [SerializeField] private TMP_Text _progressText;
        
        [Inject, UsedImplicitly] public MetaZone zone { get; }

        private void OnEnable()
        {
            Actualize(true);
            zone.changed.On(Actualize);
        }

        private void OnDisable()
        {
            zone.changed.Off(Actualize);
        }

        private void Actualize() => Actualize(false);
        
        private void Actualize(bool force)
        {
            if(force)
                _progressSlider.ForceActualize(zone.progress);
            else
                _progressSlider.value = zone.progress;
            _progressText.text = Mathf.FloorToInt(zone.progress * 100) + "%";
        }
    }
}