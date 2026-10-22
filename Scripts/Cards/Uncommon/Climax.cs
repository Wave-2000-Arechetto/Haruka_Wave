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
using MegaCrit.Sts2.Core.Models.Orbs;
using Haruka.Scripts.Enums;



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Climax : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Inspire", 1)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(HarukaKeywords.InspireKeyword)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int emotion = (int)EmotionBarManager.Instance!.Get(Owner);
            await Inspire(choiceContext, (int)DynamicVars["Inspire"].BaseValue);
            await PowerCmd.Apply<ClimaxPower>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
            
        }

        public Climax() : base(energyCost, type, rarity, targetType)
        {
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

   


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["Inspire"].UpgradeValueBy(1);
        }
    }
}