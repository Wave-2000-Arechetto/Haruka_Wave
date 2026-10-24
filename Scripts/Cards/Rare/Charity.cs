using Haruka.Scripts.Cards;
using Haruka.Scripts.Pools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

using MegaCrit.Sts2.Core.ValueProps;
using Haruka.Scripts.UI;
using MegaCrit.Sts2.Core.HoverTips;
using Haruka.Scripts.Enums;
using Test.Scripts;
using MegaCrit.Sts2.Core.Models.CardPools;



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Charity : HarukaCard
    {
        private const int energyCost = 0;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Rare;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        

        protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            

        }

        public Charity() : base(energyCost, type, rarity, targetType)
        {
        }


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars.Energy.UpgradeValueBy(2);
        }
    }
}
