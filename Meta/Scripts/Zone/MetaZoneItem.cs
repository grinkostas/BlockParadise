using System;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Saves;
using GameCore.Meta.Scripts.Currencies.Models;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Meta.Scripts.Zone
{
    public class MetaZoneItem : MonoBehaviour
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private int _price;
        [SerializeField] private string _id;
        [SerializeField] private MetaZoneItemAnimation _animation;

        public Sprite icon => _icon;
        public int price => _price;
        public string id => _id;
        public string itemName => _name;

        private TheSaveProperty<bool> _isBoughtProperty;
        private TheSaveProperty<bool> isBoughtProperty => _isBoughtProperty ??= new TheSaveProperty<bool>(_id, false);

        public bool isBought => isBoughtProperty.value;

        public TheSignal<MetaZoneItem> bought { get; } = new();

        public Tween Show() => _animation.Show();

        private void OnEnable()
        {
            if (isBought == false)
                _animation.Hide();
            else
                _animation.ForceShow();
        }

        public void Buy()
        {
            if(isBought)
                return;
            isBoughtProperty.value = true;
            bought.Dispatch(this);
        }

    }
}