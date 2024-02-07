using GameCore.Puzzle.Scripts.Fx;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.CrossScene.Scripts.UI.Fx
{
    [RequireComponent(typeof(Button))]
    public class ButtonClickFx : MonoBehaviour
    {
        [Inject, UsedImplicitly] public ClickFxHandler clickFxHandler { get; }
        
        private Button _buttonCached;
        public Button button
        {
            get
            {
                if (_buttonCached == null)
                    _buttonCached = GetComponent<Button>();
                return _buttonCached;
            }
        }

        private void OnEnable()
        {
            button.onClick.AddListener(clickFxHandler.ShowClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(clickFxHandler.ShowClick);
        }
    }
}