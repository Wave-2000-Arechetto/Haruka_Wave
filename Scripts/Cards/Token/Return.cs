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
using MegaCrit.Sts2.Core.Models.CardPools;
using Test.Scripts;
using MegaCrit.Sts2.Core.Extensions;



namespace Haruka.Scripts.Cards.Token
{
    [RegisterCard(typeof(TokenCardPool))]
    public class Return : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Token;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Move), new IntVar("Time", 12)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [
        
        CardKeyword.Exhaust
    ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            for (int i = 0; i < DynamicVars["Time"].BaseValue; i++)
            {
                float percent = Owner.RunState.Rng.CombatTargets.NextFloat();

                if (percent < 0.25f)
                {
                    await OrbCmd.Channel<BubbleOrb>(choiceContext, Owner);
                }
                if (percent < 0.5f)
                {
                    await CreatureCmd.Damage(choiceContext, Owner.Creature.CombatState!.HittableEnemies.TakeRandom(1, Owner.RunState.Rng.CombatTargets).FirstOrDefault()!, DynamicVars.Damage, Owner.Creature);
                }
                if (percent < 0.75f)
                {
                    await PlayerCmd.GainEnergy(1, Owner);
                }
                if (percent <= 1.0f)
                {
                    await CardPileCmd.Draw(choiceContext, 2, Owner);
                }
            }
            
        }

        public Return() : base(energyCost, type, rarity, targetType)
        {
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

   


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["Time"].UpgradeValueBy(8);
        }
    }
}