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



namespace Haruka.Scripts.Cards.Common
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Feathers : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Common;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;


        protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Turns", 2)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromOrb<BubbleOrb>(), HoverTipFactory.FromKeyword(HarukaKeywords.InspireKeyword)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            
            await Inspire(choiceContext, 1);
            await OrbCmd.Channel<BubbleOrb>(choiceContext, Owner);
            
            await PowerCmd.Apply<BubbleNextTurnPower>(choiceContext, Owner.Creature, DynamicVars["Turns"].BaseValue, Owner.Creature, cardPlay.Card);
            
            
        }

        public Feathers() : base(energyCost, type, rarity, targetType)
        {
        }


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["Turns"].UpgradeValueBy(1);
        }
    }
}
