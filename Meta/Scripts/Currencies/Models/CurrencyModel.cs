using GameCore.CrossScene.Scripts.Saves;
using GameCore.Meta.Scripts.Currencies.Misc;
using NepixSignals;

namespace GameCore.Meta.Scripts.Currencies.Models
{
    public abstract class CurrencyModel
    {
        private TheSaveProperty<float> _amountProperty;
        protected TheSaveProperty<float> amountProperty => _amountProperty ??= new(saveId, 0.0f);

        protected abstract string saveId { get; }
        
        public abstract CurrencyType currencyType { get; }

        public float amount
        {
            get => amountProperty.value;

            protected set
            {
                if (value <= 0)
                    amountProperty.value = 0;
                amountProperty.value = value;
            }
        } 
        
        public TheSignal<float> earned { get; } = new();
        public TheSignal<float> used { get; } = new();
        public TheSignal<float> onChange => amountProperty.changed;

        public void Earn(float earnAmount, bool callEvent = true)
        {
            if(earnAmount <= 0)
                return;
            amount += earnAmount;
            if(callEvent)
                earned.Dispatch(earnAmount);
        }
        

        public bool TryUse(float useAmount)
        {
            if(useAmount <= 0)
                return false;

            if (amount - useAmount < 0)
                return false;
            
            amount -= useAmount;
            used.Dispatch(useAmount);
            return true;
        }
        
    }
}

