using UnityEngine;

namespace GameCore.CrossScene.Scripts.Analytics
{
    public class DebugAnalytics : BaseAnalytics
    {
        public DebugAnalytics()
        {
            Load();
        }
        
        public override void SendEvent(string eventName)
        {
            Debug.Log(eventName);
        }
    }
}