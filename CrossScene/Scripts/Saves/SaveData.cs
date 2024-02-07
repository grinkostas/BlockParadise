using NepixSignals;

namespace GameCore.CrossScene.Scripts.Saves
{
    public class SaveData<T>
    {
        protected TheSaveProperty<T> property;

        private T _savedValue;
        public T savedValue
        {
            get => _savedValue;
            set
            {
                _savedValue = value;
                Save();
            }
        }
        
        public TheSignal changed { get; } = new();

        public SaveData(string key, T defaultValue = default)
        {
            property = new TheSaveProperty<T>(key, defaultValue);
            savedValue = property.value;
        }

        public void Save()
        {
            property.value = savedValue;
            changed.Dispatch();
            property.Save();
        }
    }
}