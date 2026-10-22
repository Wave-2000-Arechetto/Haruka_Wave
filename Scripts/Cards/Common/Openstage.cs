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
using Test.Scripts;
using MegaCrit.Sts2.Core.HoverTips;



namespace Haruka.Scripts.Cards.Common
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Openstage : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Attack;
        private const CardRarity rarity = CardRarity.Common;
        private const TargetType targetType = TargetType.AnyEnemy;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromOrb<SpotlightOrb>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue) // 造成伤害，数值来源于卡牌的基础伤害属性
                .FromCard(this , cardPlay) // 伤害来源于这张卡牌
                .Targeting(cardPlay.Target!) // 伤害目标是玩家选择的目标
                .Execute(choiceContext);

            await OrbCmd.Channel<SpotlightOrb>(choiceContext, Owner);
        }

        public Openstage() : base(energyCost, type, rarity, targetType)
        {
        }

        


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3); // 升级后增加4点伤害
        }
    }
}
