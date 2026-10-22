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



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class SlashTrain : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Attack;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.AnyEnemy;
        private const bool shouldShowInCardLibrary = true;
        public override bool IsMioCard => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DamageVar("Haruka",6, ValueProp.Move),
            ModCardVars.ComputedDamage(
        "Mio",
        10,
        (card, _) => card is null ? 10 : card.DynamicVars["Mio"].BaseValue,
        card => GetMio(card),
        ValueProp.Move)
            ];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            
            await DamageCmd.Attack(DynamicVars["Haruka"].BaseValue) // 造成伤害，数值来源于卡牌的基础伤害属性
                
                .FromCard(this , cardPlay) // 伤害来源于这张卡牌
                .Targeting(cardPlay.Target!)// 伤害目标是目标
                .Execute(choiceContext);

            await MioDamage(choiceContext, cardPlay.Target!, DynamicVars["Mio"].BaseValue, ValueProp.Move, 1, cardPlay.Card, cardPlay);
            

        }

        public SlashTrain() : base(energyCost, type, rarity, targetType)
        {

            
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

    
   


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            
            DynamicVars["Haruka"].UpgradeValueBy(2);
            DynamicVars["Mio"].UpgradeValueBy(2);
        }
    }
}