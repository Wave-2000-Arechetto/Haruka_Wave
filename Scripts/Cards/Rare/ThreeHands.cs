using Haruka.Scripts.Cards;
using Haruka.Scripts.Pools;
using Haruka.Scripts.Monster;
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
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Cards.DynamicVars;



namespace Haruka.Scripts.Cards.Rare
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class ThreeHands : HarukaCard
    {
        private const int energyCost = 2;
        private const CardType type = CardType.Attack;
        private const CardRarity rarity = CardRarity.Rare;
        private const TargetType targetType = TargetType.AllEnemies;
        private const bool shouldShowInCardLibrary = true;
        public override bool IsMioCard => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            ModCardVars.ComputedDamage(
        "MioDamage",
        7,
        (card, _) => card is null ? 7 : card.DynamicVars["MioDamage"].BaseValue,
        card => GetMio(card),
        ValueProp.Move),
        new PowerVar<WeakPower>(2),
        new DamageVar(0, ValueProp.Move)
        ];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await VAttack(choiceContext, DynamicVars["MioDamage"].BaseValue, cardPlay.Card, cardPlay);
            await WAttack(choiceContext, DynamicVars["MioDamage"].BaseValue, cardPlay.Card, cardPlay);
            await DAttack(choiceContext, DynamicVars["MioDamage"].BaseValue, cardPlay.Card, cardPlay);
        }

        

        public ThreeHands() : base(energyCost, type, rarity, targetType)
        {
        }


        public async Task VAttack(PlayerChoiceContext choiceContext, decimal damage, CardModel cardSource, CardPlay cardPlay)
        {
            Creature? target = Owner.Creature.CombatState!.HittableEnemies
    .Where(e => e.IsAlive && e.Monster != null)
    .OrderByDescending(x => x.CurrentHp)
    .FirstOrDefault();
            if (target != null)
            {
                await MioDamage(choiceContext, target, damage, ValueProp.Move, 1, cardSource, cardPlay);
                await PowerCmd.Apply<VulnerablePower>(choiceContext, target, 1, Owner.Creature, cardSource);
            }
        }


        public async Task WAttack(PlayerChoiceContext choiceContext, decimal damage, CardModel cardSource, CardPlay cardPlay)
        {
            Creature? target = Owner.Creature.CombatState!.HittableEnemies
    .Where(e => e.IsAlive && e.Monster != null)
    .Select(e => new
    {
        Creature = e,
        Damage = e.Monster!.NextMove.Intents
            .OfType<AttackIntent>()
            .Select(i => i.GetTotalDamage([Owner.Creature], e))
            .DefaultIfEmpty(0)
            .Max()
    })
    .OrderByDescending(x => x.Damage)
    .Select(x => x.Creature)
    .FirstOrDefault();
            if (target != null)
            {
                await MioDamage(choiceContext, target, damage, ValueProp.Move, 1, cardSource, cardPlay);
                await PowerCmd.Apply<WeakPower>(choiceContext, target, 1, Owner.Creature, cardSource);
            }
        }

        public async Task DAttack(PlayerChoiceContext choiceContext, decimal damage, CardModel cardSource, CardPlay cardPlay)
        {
            Creature? target = Owner.Creature.CombatState!.HittableEnemies
    .Where(e => e.IsAlive && e.Monster != null)
    .OrderByDescending(x => x.CurrentHp)
    .LastOrDefault();
            if (target != null)
            {
                await MioDamage(choiceContext, target, damage, ValueProp.Move, 2, cardSource, cardPlay);
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
            DynamicVars["MioDamage"].UpgradeValueBy(2);
        }
    }
}
