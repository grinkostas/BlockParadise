using NepixSignals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore.CrossScene.Scripts.UI
{
    public class SimpleToggle : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private GameObject _activeState;
        [SerializeField] private bool _isOn;

        public TheSignal<bool> onChange = new();

        public bool isOn
        {
            get => _isOn;
            set
            {
                if(_isOn != value)
                    onChange.Dispatch(value);
                _isOn = value;
                Actualize();
            }
        }
        
        private void OnValidate()
        {
            Actualize();
        }

        private void Actualize()
        {
            _activeState.SetActive(_isOn);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            isOn = !isOn;
        }
    }
}