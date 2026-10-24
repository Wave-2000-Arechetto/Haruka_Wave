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



namespace Haruka.Scripts.Cards.Rare
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Guard : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Power;
        private const CardRarity rarity = CardRarity.Rare;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        
        protected override IEnumerable<DynamicVar> CanonicalVars =>
[
    new PowerVar<MioEnhancePower>(2),
    new PowerVar<MioHpPower>(4),
];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(HarukaKeywords.MioKeyword)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [
        
    ];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner.Creature.Pets.FirstOrDefault(c => c.Monster is Mio) != null)
            {
                var mio = Owner.Creature.Pets.FirstOrDefault(c => c.Monster is Mio);
                if (mio != null)
                {
                    await CreatureCmd.GainMaxHp(mio, DynamicVars["MioHpPower"].BaseValue);
                }
                await SummonMio(choiceContext);
            }
            else
            {
                await SummonMio(choiceContext);
                
            }
            await PowerCmd.Apply<MioHpPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["MioHpPower"].BaseValue, Owner.Creature, null);
            await PowerCmd.Apply<MioEnhancePower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["MioEnhancePower"].BaseValue, Owner.Creature, null);
        }

        public Guard() : base(energyCost, type, rarity, targetType)
        {
        }


        

        


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["MioEnhancePower"].UpgradeValueBy(1);
            DynamicVars["MioHpPower"].UpgradeValueBy(2);
        }
    }
}