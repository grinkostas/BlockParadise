using System;
using GameCore.CrossScene.Scripts.Analytics;
using GameCore.Meta.Scripts.Zone;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Meta.Analytics
{
    public class ZoneItemsAnalytics : MonoBehaviour
    {
        [Inject, UsedImplicitly] public MetaZone zone { get; }
        [InjectOptional, UsedImplicitly] public IAnalytics analytics { get; }
        
        private void Awake()
        {
            zone.bought.On(OnZoneItemBought);
        }

        private void OnZoneItemBought(MetaZoneItem item)
        {
            analytics?.SendEvent(item.id, "Meta");
        }
    }
}