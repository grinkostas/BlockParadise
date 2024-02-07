using System;
using DG.Tweening;
using GameCore.CrossScene.Scripts.UI.Popups;
using NaughtyAttributes;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.Sounds.Settings
{
    public class SoundSettingsPopup : TypeInjectablePopup
    {
        protected override Type popupType => typeof(SoundSettingsPopup);
        
    }
}