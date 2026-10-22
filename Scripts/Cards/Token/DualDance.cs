using Haruka.Scripts.Cards;
using Haruka.Scripts.Pools;
using Haruka.Scripts.Enums;
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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Orbs;
using System.Reflection;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using Godot;
using Test.Scripts;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(TokenCardPool))]
    public class DualDance : HarukaCard
    {
        private const int energyCost = 0;
        private const CardType type = CardType.Attack;
        private const CardRarity rarity = CardRarity.Token;
        private const TargetType targetType = TargetType.AnyEnemy;
        private const bool shouldShowInCardLibrary = true;
        public override bool IsMioCard => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            ModCardVars.ComputedDamage(
        "MioDamage",
        9,
        (card, _) => card is null ? 9 : card.DynamicVars["MioDamage"].BaseValue,
        card => GetMio(card),
        ValueProp.Move)
            ];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromOrb<BubbleOrb>()];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Retain];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            
            await OrbCmd.Channel<BubbleOrb>(choiceContext, Owner);

            await MioDamage(choiceContext, cardPlay.Target!, DynamicVars["MioDamage"].BaseValue, ValueProp.Move, 1, cardPlay.Card, cardPlay);
            

        }

        public DualDance() : base(energyCost, type, rarity, targetType)
        {

            
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

    
   


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            
            
            DynamicVars["MioDamage"].UpgradeValueBy(3);
        }
    }
}