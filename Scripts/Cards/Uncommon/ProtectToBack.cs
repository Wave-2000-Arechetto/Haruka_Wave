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



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Veiled : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromOrb<BubbleOrb>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await OrbCmd.Channel<BubbleOrb>(choiceContext, Owner);

            while (Owner.PlayerCombatState!.OrbQueue.Orbs[0] is BubbleOrb || Owner.PlayerCombatState!.OrbQueue.Orbs[0] == null)
            {
                await OrbCmd.EvokeNext(choiceContext, Owner);
            }
        }

        public Veiled() : base(energyCost, type, rarity, targetType)
        {
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

   


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["Block"].UpgradeValueBy(3);
            
        }
    }
}