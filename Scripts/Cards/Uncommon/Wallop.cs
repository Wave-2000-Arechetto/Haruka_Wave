
using Haruka.Scripts.Cards;
using Haruka.Scripts.Pools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

using MegaCrit.Sts2.Core.ValueProps;

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Combat;
using STS2RitsuLib.Scaffolding.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.HoverTips;

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;

using MegaCrit.Sts2.Core.Commands.Builders;

using Haruka.Scripts.UI;
using Haruka.Scripts.Enums;


namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Wallop : HarukaCard
    {
        private const int energyCost = 1 ;
        private const CardType type = CardType.Attack;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.AnyEnemy;
        private const bool shouldShowInCardLibrary = true;

        

        protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(7m),
		new ExtraDamageVar(2m),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
            (CardModel card, Creature? _) => GetEmotionMultiplier(card, _)
        )];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [
        
        
    ];
        protected override IEnumerable<IHoverTip> AdditionalHoverTips
 => [
            ModKeywordRegistry.CreateHoverTip(HarukaKeywords.CalmKeyword)
        ];

        private static int GetEmotionMultiplier(CardModel card, Creature? _)
{
    if (card?.Owner is not Player player)
        return 0;

    var manager = EmotionBarManager.Instance;
    if (manager == null)
        return 0;   // 战斗未就绪时按平静算，预览显示基础伤害

    return (int)Math.Abs(manager.Get(player));
}

        
        
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null)
            {
                return;
            }

            
            if (IsPlayable)
            {
                AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.CalculatedDamage.Calculate(target)) // 造成伤害，数值来源于卡牌的基础伤害属性
                    .FromCard(this, cardPlay) // 伤害来源于这张卡牌
                    .Targeting(target) // 伤害目标是玩家选择的目标
                    .Execute(choiceContext);

                await CreatureCmd.GainBlock(base.Owner.Creature, attackCommand.Results.SelectMany((List<DamageResult> r) => r).Sum((DamageResult r) => r.UnblockedDamage), ValueProp.Move, cardPlay, false);
            }
        }


        

        

        
        
        public Wallop() : base(energyCost, type, rarity, targetType)
        {
        }

        
        
        

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars.ExtraDamage.UpgradeValueBy(1);
        }
    }
}