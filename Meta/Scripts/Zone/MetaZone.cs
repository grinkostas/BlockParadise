using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NepixSignals;
using UnityEngine;

namespace GameCore.Meta.Scripts.Zone
{
    public class MetaZone : MonoBehaviour
    {
        [SerializeField] private string _zoneId;
        [SerializeField] private string _zoneName;
        [SerializeField] private List<MetaZoneItem> _zoneItems;

        public string zoneName => _zoneName;

        public MetaZoneItem nextItem => _zoneItems.Find(x => x.isBought == false);

        public int totalItemsCount => _zoneItems.Count;
        public int completedItemsCount => _zoneItems.Count(x => x.isBought);
        public float progress => completedItemsCount / (float)totalItemsCount;
        
        public TheSignal changed { get; } = new();
        public TheSignal<MetaZoneItem> bought { get; } = new();

        public void BuyNextItem()
        {
            bought.Dispatch(nextItem);
            nextItem.Buy();
            changed.Dispatch();
        }
    }
}