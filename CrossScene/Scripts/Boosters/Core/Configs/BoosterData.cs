using UnityEngine;

namespace GameCore.CrossScene.Scripts.Boosters.Core.Configs
{
    [CreateAssetMenu(menuName = "Boosters/Data")]
    public class BoosterData : ScriptableObject
    {
        public string id;
        public Sprite icon;
        public int price;
    }
}