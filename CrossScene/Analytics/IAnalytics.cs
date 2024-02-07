using NepixSignals;

namespace GameCore.CrossScene.Scripts.Analytics
{
    public interface IAnalytics
    {
        public bool isLoaded { get; protected set; }
        public TheSignal loaded { get; }
        public void SendEvent(string eventName);
        public void SendEvent(string eventName, string eventGroup);
        public void SendComplete(string eventName);
        public void SendComplete(string eventName, string eventGroup);
        public void SendFailed(string eventName);
        public void SendFailed(string eventName, string eventGroup);
    }
}