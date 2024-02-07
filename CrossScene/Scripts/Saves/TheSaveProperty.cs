using NepixSignals;

namespace GameCore.CrossScene.Scripts.Saves
{
    public class TheSaveProperty<T>
    {
        private string _key;
        private T _defaultValue = default;
        
        private bool _loaded = false;
        
        protected T _value;
        public T value
        {
            get
            {
                if(!_loaded) Load();
                return _value;
            }
            set
            {
                _value = value;
                Save();
                changed.Dispatch(_value);
            }
        }

        public TheSignal<T> changed { get; } = new();

        public TheSaveProperty(string key, T defaultValue = default)
        {
            _key = key;
            _defaultValue = defaultValue;
        }

        private void Load()
        {
            _loaded = true;
            _value = ES3.Load(_key, defaultValue:_defaultValue);
        }

        public void Save()
        {
            ES3.Save(_key, _value);
        }
    }
}