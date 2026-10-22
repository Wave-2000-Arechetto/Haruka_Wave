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
using Haruka.Scripts.Cards.Token;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.CardSelection;



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class GlassShop : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Min", 1)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int minGlass = DynamicVars["Min"].IntValue;
            int maxGlass = 6;

            for (int i = 0; i<2; i++)
            {
                int num = Owner.RunState.Rng.CombatTargets.NextInt(minGlass - 1, maxGlass + 1);
                OrbModel glass = ModelDb.Orb<GlassOrb>().ToMutable();
                if (glass is GlassOrb glassOrb && num > 0)
                {
                    glassOrb._passiveVal = num;
                    await OrbCmd.Channel(choiceContext, glass, Owner);
                }
            }


        }

        public GlassShop() : base(energyCost, type, rarity, targetType)
        {
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

   


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["Min"].UpgradeValueBy(2);
        }
    }
}