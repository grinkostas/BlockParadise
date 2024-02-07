using GameCore.Meta.Scripts.Currencies.Misc;

namespace GameCore.Meta.Scripts.Currencies.Models
{
    public class StarCollectModel : CurrencyModel
    {
        protected override string saveId => "StarCollectModelAmount";
        public override CurrencyType currencyType => CurrencyType.Stars;
    }
}