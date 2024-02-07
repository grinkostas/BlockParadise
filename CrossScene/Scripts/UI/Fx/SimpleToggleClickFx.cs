using GameCore.Puzzle.Scripts.Fx;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.CrossScene.Scripts.UI.Fx
{
    [RequireComponent(typeof(SimpleToggle))]
    public class SimpleToggleClickFx : MonoBehaviour
    {
        [Inject, UsedImplicitly] public ClickFxHandler clickFxHandler { get; }
        
        private SimpleToggle _toggleCached;
        public SimpleToggle toggle
        {
            get
            {
                if (_toggleCached == null)
                    _toggleCached = GetComponent<SimpleToggle>();
                return _toggleCached;
            }
        }

        private void OnEnable()
        {
            toggle.onChange.On(OnToggleChange);
        }

        private void OnDisable()
        {
            toggle.onChange.Off(OnToggleChange);
        }

        private void OnToggleChange(bool _) => clickFxHandler.ShowClick();
    }
}