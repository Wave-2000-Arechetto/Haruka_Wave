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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;
using Haruka.Scripts.Enums;
using Haruka.Scripts.Cards.Token;
using Test.Scripts;




namespace Haruka.Scripts.Cards.Ancient
{
    [RegisterCard(typeof(HarukaCardPool))]
    
    public class Higashi : HarukaCard 
    {

        
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Ancient;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Time",1)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromOrb<BubbleOrb>(), HoverTipFactory.FromOrb<FireworkOrb>(),HoverTipFactory.FromOrb<SpotlightOrb>()];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await OrbCmd.Channel<BubbleOrb>(choiceContext, Owner);
            await OrbCmd.Channel<FireworkOrb>(choiceContext, Owner);
            await OrbCmd.Channel<SpotlightOrb>(choiceContext, Owner);

            for (int i = 0; i < DynamicVars["Time"].IntValue; i++)
            {
                foreach (OrbModel orb in Owner.PlayerCombatState?.OrbQueue?.Orbs ?? [])
                {
                    orb?.ActivatePassive();
                }
            }
        }

        public Higashi() : base(energyCost, type, rarity, targetType)
        {
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
            ];


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["Time"].UpgradeValueBy(1);
        }
    }
}
