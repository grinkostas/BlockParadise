using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Meta.Scripts.Tutorial
{
    public class TutorialContainer : MonoBehaviour
    {
        [Inject, UsedImplicitly] public MetaTutorial metaTutorial { get; }
        
        private void OnEnable()
        {
            if (metaTutorial.isStarted != false) return;
            
            metaTutorial.started.Once(() => gameObject.SetActive(true));
            gameObject.SetActive(false);
        }
    }
}