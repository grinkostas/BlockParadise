using System;
using DG.Tweening;
using Febucci.UI;
using StaserSDK.Extentions;
using TMPro;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.UI
{
    public class TextWriter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _textToSet;
        [SerializeField] private float _characterDelay;
        [SerializeField] private float _delay;

        private void OnEnable()
        {
            ResetText();
            AnimateText();
        }

        private void ResetText()
        {
            _text.text = "";
        }

        private void AnimateText()
        {
            DOVirtual.Int(1, _textToSet.Length, _characterDelay*(_textToSet.Length-1), value =>
            {
                _text.text = _textToSet[..value];
            }).SetDelay(_delay).ConfigureWithId(this, gameObject);
        }

        private void OnDisable()
        {
            DOTween.Kill(this);
        }
    }
}