using GameCore.CrossScene.Scripts.Boosters.Core.Configs;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GameCore.CrossScene.Scripts.Boosters.Core
{
    public abstract class Booster : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] protected BoosterData data;

        [Inject, UsedImplicitly] public BoostersController boostersController { get; }
        
        public string id => data.id;
        
        public TheSignal selected { get; } = new();
        public TheSignal used { get; } = new();

        
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Selected");
            selected.Dispatch();
            OnClick();
        }
        
        protected abstract void OnClick();

        protected void Use()
        {
            used.Dispatch();
            boostersController.UseBooster(data.id);
        }
    }
}