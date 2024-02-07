using NepixSignals;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.Analytics
{
    public abstract class BaseAnalytics : IAnalytics
    {
        private bool _isLoaded;

        bool IAnalytics.isLoaded
        {
            get => _isLoaded;
            set => _isLoaded = value;
        }

        public TheSignal loaded { get; } = new();
        
        public abstract void SendEvent(string eventName);
        
        public void SendEvent(string eventName, string eventGroup)
        {
            SendEvent($"{eventGroup}:{eventName}");
        }

        public void SendComplete(string eventName, string eventGroup)
        {
            SendEvent($"compete:{eventName}", eventGroup);
        }
        public void SendComplete(string eventName)
        {
            SendEvent($"compete:{eventName}");
        }

        public void SendFailed(string eventName, string eventGroup)
        {
            SendEvent($"failed:{eventName}", eventGroup);
        }
        public void SendFailed(string eventName)
        {
            SendEvent($"failed:{eventName}");
        }

        public void Load()
        {
            _isLoaded = true;
            loaded.Dispatch();
        }
    }
}