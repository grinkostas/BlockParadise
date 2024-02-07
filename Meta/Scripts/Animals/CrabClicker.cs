using GameCore.Puzzle.Scripts.Bindings;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Meta.Scripts.Animals
{
    public class CrabClicker : MonoBehaviour
    {
        [Inject, UsedImplicitly] public MainCamera camera { get; }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Click();
            }
        }

        private void Click()
        {
            RaycastHit hit;
            Ray ray = camera.cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 1000f) == false)
                return;
            
            if (hit.collider.TryGetComponent(out CrabController crab))
                crab.Victory();
        }
    }
}