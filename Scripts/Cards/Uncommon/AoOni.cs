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
using Test.Scripts;
using Haruka.Scripts.Enums;



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class AoOni : HarukaCard
    {
        private const int energyCost = 2;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12, ValueProp.Move), new IntVar("Extra", 1)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(HarukaKeywords.ScareKeyword), HoverTipFactory.FromOrb<BubbleOrb>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int emotion = (int)EmotionBarManager.Instance!.Get(Owner);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await Scare(choiceContext, 2);
            if (emotion >= 0 && (int)EmotionBarManager.Instance!.Get(Owner) < 0)
            {
                for (int i = 0; i < DynamicVars["Extra"].BaseValue; i++)
                {
                    await OrbCmd.Channel<BubbleOrb>(choiceContext, Owner);
                }
            }
        }

        public AoOni() : base(energyCost, type, rarity, targetType)
        {
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

   


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["Block"].UpgradeValueBy(2);
            DynamicVars["Extra"].UpgradeValueBy(1); 
        }
    }
}