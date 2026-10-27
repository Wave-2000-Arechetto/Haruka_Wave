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
using MegaCrit.Sts2.Core.Models;
using Haruka.Scripts.Cards.Token;
using Test.Scripts;
using MinionLib.Commands;
using System.Numerics;
using MinionLib.Minion;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;



namespace Haruka.Scripts.Cards.Rare
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class CertainFate : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Power;
        private const CardRarity rarity = CardRarity.Rare;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Slots", 1)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromOrb<CertainFateOrb>()];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            
            
            await OrbCmd.AddSlots(Owner, DynamicVars["Slots"].IntValue);
            await OrbCmd.Channel<CertainFateOrb>(choiceContext, Owner);
        }

        public CertainFate() : base(energyCost, type, rarity, targetType)
        {
        }


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["Slots"].UpgradeValueBy(1);
            
        }
    }
}
