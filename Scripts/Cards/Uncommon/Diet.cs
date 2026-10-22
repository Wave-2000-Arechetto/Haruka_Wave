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



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Diet : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Attack;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.AnyEnemy;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12, ValueProp.Move), new IntVar("Less", 1), new IntVar("Amount", 1)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromOrb<BubbleOrb>()];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue) // 造成伤害，数值来源于卡牌的基础伤害属性
                .FromCard(this , cardPlay) // 伤害来源于这张卡牌
                .Targeting(cardPlay.Target!)// 伤害目标是目标
                .Execute(choiceContext);

            for (int i = 0; i < DynamicVars["Amount"].BaseValue; i++)    
            {
                await OrbCmd.Channel<BubbleOrb>(choiceContext, Owner);
            }

            await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, Owner.Creature, -DynamicVars["Less"].BaseValue, Owner.Creature, null);
        }

        public Diet() : base(energyCost, type, rarity, targetType)
        {

            
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

    
   


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            
            DynamicVars["Amount"].UpgradeValueBy(1);
        }
    }
}