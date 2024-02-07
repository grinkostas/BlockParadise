using GameCore.CrossScene.Scripts.Boosters.Core.Configs;
using NepixSignals;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.Boosters
{
    public class BoostersController : MonoBehaviour
    {
        [SerializeField] private BoostersCollection _boostersCollection;

        public TheSignal<string> usedBoosted { get; } = new();
        
        public int GetCount(string boosterId)
        {
            if (_boostersCollection.boosters.Has(x => x.id == boosterId) == false)
                return 0;
            return ES3.Load(boosterId, 5);
        }

        public void UseBooster(string boosterId)
        {
            if (_boostersCollection.boosters.Has(x => x.id == boosterId) == false)
                return;
            int newCount = Mathf.Max(0, GetCount(boosterId) - 1);
            ES3.Save(boosterId, newCount);
            usedBoosted.Dispatch(boosterId);
        }
    }
}