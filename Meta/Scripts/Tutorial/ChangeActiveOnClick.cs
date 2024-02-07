using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Meta.Scripts.Tutorial
{
    public class ChangeActiveOnClick : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private List<GameObject> _objects;
        [SerializeField] private bool _targetActive;
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            foreach (var disableObject in _objects)
            {
                disableObject.SetActive(_targetActive);
            }
        }
    }
}