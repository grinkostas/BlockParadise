using System;
using System.Collections.Generic;

namespace GameCore.CrossScene.Scripts.UI.Popups
{
    public class PopupManager
    {
        private Dictionary<Type, Popup> _typedPopups = new();
        private Dictionary<string, Popup> _idPopups = new();
        
        public PopupManager(){}

        public void Add(Type type, Popup popup)
        {
            if(_typedPopups.ContainsKey(type))
                return;
            _typedPopups.Add(type, popup);
        }

        public bool TryGet<T>(out Popup popup)
        {
            popup = null;
            if (_typedPopups.ContainsKey(typeof(T)) == false)
                return false;
            popup = _typedPopups[typeof(T)];
            return true;
        }
        
        public void Add(string id, Popup popup)
        {
            if(_idPopups.ContainsKey(id))
                return;
            _idPopups.Add(id, popup);
        }

        public bool TryGet(string id, out Popup popup)
        {
            popup = null;
            if (_idPopups.ContainsKey(id) == false)
                return false;
            popup = _idPopups[id];
            return true;
        }
    }
}