using UnityEngine;

namespace GameCore.Puzzle.Scripts.Bindings
{
    [RequireComponent(typeof(Camera))]
    public class MainCamera : MonoBehaviour
    {
        private Camera _cameraCached;
        public Camera cam
        {
            get
            {
                if (_cameraCached == null)
                    _cameraCached = GetComponent<Camera>();
                return _cameraCached;
            }
        }
    }
}