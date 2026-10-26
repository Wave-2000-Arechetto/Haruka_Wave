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
using Haruka.Scripts.Enums;
using MegaCrit.Sts2.Core.Models;
using Haruka.Scripts.Cards.Token;
using Test.Scripts;
using MegaCrit.Sts2.Core.Entities.Creatures;



namespace Haruka.Scripts.Cards.Rare
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Ruin : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Attack;
        private const CardRarity rarity = CardRarity.Rare;
        private const TargetType targetType = TargetType.AnyEnemy;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Move)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue) // 造成伤害，数值来源于卡牌的基础伤害属性
                .FromCard(this , cardPlay) // 伤害来源于这张卡牌
                .Targeting(cardPlay.Target!) // 伤害目标是玩家选择的目标
                .Execute(choiceContext);
            int num = Owner?.PlayerCombatState?.OrbQueue?.Orbs.Count ?? 0;
            foreach (OrbModel orb in Owner?.PlayerCombatState?.OrbQueue?.Orbs.ToList() ?? [])
{
    if (orb == null) continue;
    if (!Owner!.PlayerCombatState!.OrbQueue.Orbs.Contains(orb)) continue; // 已被连锁移除的跳过
    try { await OrbCmd.Evoke(choiceContext, Owner!, orb); }
    catch (Exception e) {}
}
            await OrbCmd.AddSlots(Owner!, num);
        }

        

        public Ruin() : base(energyCost, type, rarity, targetType)
        {
        }


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4);
        }
    }
}
