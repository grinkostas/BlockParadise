using System.Collections.Generic;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.Boosters.Core.Configs
{
    [CreateAssetMenu(menuName = "Boosters/Collection")]
    public class BoostersCollection : ScriptableObject
    {
        public List<BoosterData> boosters;
    }
}