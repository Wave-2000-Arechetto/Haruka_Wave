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



namespace Haruka.Scripts.Cards.Rare
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class CoPerformance : HarukaCard
    {
        private const int energyCost = 2;
        private const CardType type = CardType.Power;
        private const CardRarity rarity = CardRarity.Rare;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        

        protected override IEnumerable<DynamicVar> CanonicalVars => [];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromCard<DualDance>(IsUpgraded),HoverTipFactory.FromOrb<BubbleOrb>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (IsUpgraded)
            {
                await PowerCmd.Apply<CoPerformanceUpgradePower>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
            }
            else
            {
                await PowerCmd.Apply<CoPerformancePower>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
            }
            

        }

        public CoPerformance() : base(energyCost, type, rarity, targetType)
        {
        }


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            
        }
    }
}
