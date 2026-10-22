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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Kichisei : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Attack;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.AnyEnemy;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var entries = CombatManager.Instance.History.Entries.ToList();
    GD
.Print($"[吉星] 总条目: {entries.Count}");
    var orbs = entries.OfType<OrbChanneledEntry>().ToList();
    GD
.Print($"[吉星] 球条目: {orbs.Count}");
    foreach (var e in orbs)
        GD
.Print($"[吉星]   {e.Orb?.GetType().Name} owner={e.Orb?.Owner?.GetHashCode()} 我的Owner={Owner.GetHashCode()}");
    
    int count =
 orbs
        .Where(e => e.Orb.Owner == Owner)
        .Select(e => e.Orb.GetType())
        .Distinct()
        .Count();
    GD
.Print($"[吉星] count={count}");

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(count)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
            .Execute(choiceContext);

        }

        public Kichisei() : base(energyCost, type, rarity, targetType)
        {

            
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

    
   


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            
            DynamicVars.Damage.UpgradeValueBy(2);
        }
    }
}