using DG.Tweening;
using GameCore.CrossScene.Scripts.UI;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.Pools
{
    public class CanvasAnimateItemPool : MonoBehaviour
    {
        [SerializeField] private MonoPool<CanvasAnimateItem> _pool;
        [SerializeField] private float _returnDelay;

        public MonoPool<CanvasAnimateItem> pool
        {
            get
            {
                if(_pool.initialized == false)
                    _pool.Initialize();
                return _pool;
            }
        }

        private void Awake()
        {
            _pool.Initialize();
        }

        public CanvasAnimateItem Get()
        {
            var item = pool.Get();
            DOVirtual.DelayedCall(_returnDelay, () => Return(item));
            return item;
        }
        public void Return(CanvasAnimateItem item) => item.Pool.Return(item);
    }
}