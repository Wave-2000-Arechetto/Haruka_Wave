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
using MegaCrit.Sts2.Core.Combat;



namespace Haruka.Scripts.Cards.Token
{
    [RegisterCard(typeof(TokenCardPool))]
    public class Lai : HarukaCard
    {
        private const int energyCost = 3;
        private const CardType type = CardType.Attack;
        private const CardRarity rarity = CardRarity.Token;
        private const TargetType targetType = TargetType.AnyEnemy;
        private const bool shouldShowInCardLibrary = true;

        public override bool IsMioCard => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
[
    ModCardVars.ComputedDamage(
        "MioDamage",
        25,
        (card, _) => card is null ? 25 : card.DynamicVars["MioDamage"].BaseValue,
        card => GetMio(card),
        ValueProp.Move)
];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Retain,
        CardKeyword.Exhaust
    ];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await MioDamage(choiceContext, cardPlay.Target!, DynamicVars["MioDamage"].BaseValue, ValueProp.Move, 1, cardPlay.Card, cardPlay);
        }

        public Lai() : base(energyCost, type, rarity, targetType)
        {
        }

        public override async Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (side == CombatSide.Player)
            {
                EnergyCost.UpgradeBy(-1);
            }
        }

        

        protected override bool IsPlayable
        {
            get
            {
                if (Owner.Creature.Pets.FirstOrDefault(c => c.Monster is Mio) == null)
                {
                    return false;
                }
                return true;
            }
        }


        

        


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["MioDamage"].UpgradeValueBy(8); // 升级后增加8点伤害
        }
    }
}