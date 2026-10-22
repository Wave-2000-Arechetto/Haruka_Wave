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
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using Haruka.Scripts.Monster;
using STS2RitsuLib.Cards.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Haruka.Scripts.Enums;



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Determined : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Power;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        
        protected override IEnumerable<DynamicVar> CanonicalVars =>
[
    new PowerVar<DeterminedPower>(3),
];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [
        
    ];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<DeterminedPower>(choiceContext, Owner.Creature, DynamicVars["DeterminedPower"].BaseValue, Owner.Creature, null);
        }

        public Determined() : base(energyCost, type, rarity, targetType)
        {
        }


        

        


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["DeterminedPower"].UpgradeValueBy(1);
        }
    }
}