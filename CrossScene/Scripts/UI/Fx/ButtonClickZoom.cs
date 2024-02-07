using DG.Tweening;
using GameCore.CrossScene.Scripts.Sounds.SoundPools;
using Haptic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.CrossScene.Scripts.UI.Fx
{
    [RequireComponent(typeof(Button))]
    public class ButtonClickZoom : MonoBehaviour
    {
        [SerializeField] private float _punchMultiplayer = 1.0f;
        [SerializeField] private bool _haptic = true;
        [SerializeField] private bool _sound = true;

        [InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }
        [InjectOptional, UsedImplicitly] public ClickSoundPool clickSoundPool { get; }
        
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
            button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
        }

        [NaughtyAttributes.Button()]
        private void OnClick()
        {
            if(DOTween.IsTweening(this))
                return;
            if(_haptic)
                hapticService?.Selection();
            if(_sound && clickSoundPool != null)
                clickSoundPool.PlaySound();
            button.transform.DOPunchScale(Vector3.one * (0.15f*_punchMultiplayer) , 0.35f, 2)
                .SetUpdate(false).SetLink(gameObject).SetId(this);
        }
    }
}