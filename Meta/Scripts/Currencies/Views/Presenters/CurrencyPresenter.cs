using System.Collections.Generic;
using GameCore.Meta.Scripts.Currencies.Models;
using GameCore.Meta.Scripts.Currencies.Views.Displays;
using Zenject;

namespace GameCore.Meta.Scripts.Currencies.Views.Presenters
{
    public abstract class CurrencyPresenter
    {
        private List<CurrencyDisplay> _displays = new List<CurrencyDisplay>();

        public abstract CurrencyModel model { get; }

        public CurrencyDisplay mainDisplay { get; private set; }

        [Inject]
        public void Construct()
        {
            model.onChange.On(OnChange);
        }

        public void AddDisplay(CurrencyDisplay display, bool main = false)
        {
            if(main)
                mainDisplay = display;
            if(_displays.Contains(display))
                return;
            if (mainDisplay == null)
                mainDisplay = display;
            _displays.Add(display);
            display.SetAmount(model.amount);
        }
        

        private void OnChange(float amount)
        {
            foreach (var display in _displays)
            {
                display.SetAmount(amount);
            }
        }
        
    }
}