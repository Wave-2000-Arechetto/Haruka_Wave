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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Orbs;
using System.Reflection;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using Godot;



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class StageRotation : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var orbQueue = Owner.PlayerCombatState?.OrbQueue;
    if (orbQueue?.Orbs.Count < 2) return;

    // ========== 1. 交换数据层 ==========
    var dataField = typeof(OrbQueue).GetField("_orbs",
        BindingFlags.NonPublic | BindingFlags.Instance);
    var dataList = dataField?.GetValue(orbQueue) as List<OrbModel>;
    if (dataList == null) return;

    (dataList[0], dataList[1]) = (dataList[1], dataList[0]);

    // ========== 2. 交换表现层 ==========
    var creatureNode = NCombatRoom.Instance?.GetCreatureNode(Owner.Creature);
    var orbManager = creatureNode?.OrbManager;
    if (orbManager == null) return;

    // 获取 NOrbManager 的私有字段
    var nOrbsField = typeof(NOrbManager).GetField("_orbs",
        BindingFlags.NonPublic | BindingFlags.Instance);
    var nOrbs = nOrbsField?.GetValue(orbManager) as List<NOrb>;
    
    var containerField = typeof(NOrbManager).GetField("_orbContainer",
        BindingFlags.NonPublic | BindingFlags.Instance);
    var container = containerField?.GetValue(orbManager) as Control;

    if (nOrbs != null && nOrbs.Count >= 2)
    {
        // 交换 NOrb 节点在列表中的顺序
        (nOrbs[0], nOrbs[1]) = (nOrbs[1], nOrbs[0]);

        // 同步 Godot 子节点顺序（保持 _orbContainer 和 _orbs 一致）
        container?.MoveChild(container.GetChild(1), 0);

        // 触发飞行动画
        typeof(NOrbManager).GetMethod("TweenLayout",
            BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(orbManager, null);

        // 更新手柄导航
        typeof(NOrbManager).GetMethod("UpdateControllerNavigation",
            BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(orbManager, null);
            }

        await orbQueue!.Orbs[0].Evoke(choiceContext); 
        await orbQueue!.Orbs[1].Evoke(choiceContext); 
            
        }

        public StageRotation() : base(energyCost, type, rarity, targetType)
        {

            
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

    
   


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}