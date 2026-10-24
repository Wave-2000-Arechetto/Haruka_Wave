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
using MegaCrit.Sts2.Core.Models;



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class BubbleWave : HarukaCard
    {
        private const int energyCost = 0;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override bool HasEnergyCostX => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            
            
            int x = ResolveEnergyXValue();
            if (IsUpgraded){
                x += 1;
            }

        for (int i = 0; i < x; i++)
            {
                OrbModel b = ModelDb.Orb<BubbleOrb>().ToMutable();
                await OrbCmd.Channel(choiceContext,b,Owner);
            }
            
        }

        public BubbleWave() : base(energyCost, type, rarity, targetType)
        {
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

   


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
             
        }
    }
}