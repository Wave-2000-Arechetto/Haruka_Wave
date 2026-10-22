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
    public class GhostHand : HarukaCard
    {
        private const int energyCost = 2;
        private const CardType type = CardType.Attack;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.AllEnemies;
        private const bool shouldShowInCardLibrary = true;

        public override bool IsMioCard => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
[
    ModCardVars.ComputedDamage(
        "MioDamage",
        13,
        (card, _) => card is null ? 13 : card.DynamicVars["MioDamage"].BaseValue,
        card => GetMio(card),
        ValueProp.Move),
        new PowerVar<WeakPower>(2)
];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(HarukaKeywords.MioKeyword), HoverTipFactory.FromPower<WeakPower>()];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [
        
    ];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await MioDamage(choiceContext, CombatState!.HittableEnemies, DynamicVars["MioDamage"].BaseValue, ValueProp.Move, 1, cardPlay.Card, cardPlay);
            foreach(Creature target in CombatState!.HittableEnemies)
            {
                await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), target!, DynamicVars["WeakPower"].BaseValue, Owner.Creature, null);
            }
        }

        public GhostHand() : base(energyCost, type, rarity, targetType)
        {
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
            DynamicVars["MioDamage"].UpgradeValueBy(2);
            DynamicVars["WeakPower"].UpgradeValueBy(1);
        }
    }
}