using System;

namespace GameCore.CrossScene.Scripts.UI.Popups
{
    public abstract class TypeInjectablePopup : Popup
    {
        protected abstract Type popupType { get; }
        
        protected override void OnInject()
        {
            base.OnInject();
            popupManager.Add(popupType, this);
        }
    }
}