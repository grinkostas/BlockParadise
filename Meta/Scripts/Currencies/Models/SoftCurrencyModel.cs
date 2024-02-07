using GameCore.Meta.Scripts.Currencies.Misc;

namespace GameCore.Meta.Scripts.Currencies.Models
{
    public class SoftCurrencyModel : CurrencyModel
    {
        protected override string saveId => "SoftCurrencyAmount";
        public override CurrencyType currencyType => CurrencyType.Soft;
    }
}