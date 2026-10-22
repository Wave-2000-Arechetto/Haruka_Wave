using System.Reflection;
using Godot;
using HarmonyLib;
using Haruka.Scripts.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;

namespace Test.Scripts;

[RegisterOrb]
public class CertainFateOrb : ModOrbTemplate
{

    private decimal _passiveVal = 0m;
    // 被动效果数值，ModifyOrbValue表示是否吃集中等
    public override decimal PassiveVal => 0;

    // 激发效果数值
    public override decimal EvokeVal => 0;

    public override bool AllowInRandomOrbPool => false;

    // 战斗中充能球的文本显示样式。
    // Contextual 表示匹配普通原版充能球行为：平时显示被动值；当充能球被预览为激发时显示激发值。一般用于大多数充能球。
    // SinglePassive 表示只显示被动值。
    // SingleEvoke 表示只显示激发值。
    // Both 表示同时显示被动值和激发值。原版用于黑暗充能球。
    public override ModOrbValueDisplayMode ValueDisplayMode => ModOrbValueDisplayMode.Hidden;

    // 暗色，使用球的主体色的暗色调
    public override Color DarkenedColor => new(0.1f, 0.2f, 0.5f);

    // 对于图片，只要是godot支持的格式都可以，例如png,jpg,svg等等
    public override OrbAssetProfile AssetProfile => new(
        // 提示文本小图标路径
        IconPath: "res://Haruka/images/orbs/certain_fate_orb.png",
        // 充能球场景路径
        VisualsScenePath: "res://Haruka/scenes/certain_fate_orb.tscn"
    );

    // 让你不需要手动挂脚本。复制即可。
    protected override Node2D? TryCreateOrbSprite() => RitsuGodotNodeFactories.CreateFromScenePath<Node2D>(AssetProfile.VisualsScenePath!);

    // 回合开始时触发被动
    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
	{
		await TriggerPassive(choiceContext, null);
	}

    // 触发被动
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        OrbModel orb = (OrbModel)Owner.PlayerCombatState!.OrbQueue.Orbs[0].ClonePreservingMutability();
        if (orb == null)
        {
            return;
        }
        else
        {
            if (orb is not CertainFateOrb)
            {
                await OrbCmd.EvokeNext(choiceContext, Owner);
                await Cmd.Wait(0.1f);
                await OrbCmd.Channel(choiceContext, orb, Owner);
            }
        }
    }

    

    

    // 触发激发，返回受影响的角色
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        PlayEvokeSfx();
        for (int i = 0; i < Owner.PlayerCombatState!.OrbQueue.Orbs.Count(); i++)
        {
            OrbModel orb = (OrbModel)Owner.PlayerCombatState!.OrbQueue.Orbs[Owner.PlayerCombatState!.OrbQueue.Orbs.Count-1].ClonePreservingMutability();
            if (orb == null)
            {
                return[Owner.Creature];
            }
            else
            {
            
                await OrbCmd.EvokeLast(playerChoiceContext, Owner);
                await Cmd.Wait(0.1f);
                if (orb is not CertainFateOrb)
                {
                    await ChannelOnFront(playerChoiceContext, orb);
                }       
            
            }
        }
        return[Owner.Creature];
    }


    public async Task ChannelOnFront(PlayerChoiceContext choiceContext, OrbModel orbModel)
{
    var player = Owner;
    var orbQueue = player.PlayerCombatState?.OrbQueue;
    if (orbQueue == null || orbQueue.Capacity == 0) return;

    // ========== 0. 设置 Owner（关键修复） ==========
    var ownerField = typeof(AbstractModel).GetField("_owner",
        BindingFlags
.NonPublic | BindingFlags.Instance);
    ownerField
?.SetValue(orbModel, player);

    // ========== 1. 数据层：头部插入，尾部挤出 ==========
    var dataField = typeof(OrbQueue).GetField("_orbs",
        BindingFlags
.NonPublic | BindingFlags.Instance);
    var dataList = dataField?.GetValue(orbQueue) as List<OrbModel>;
    if (dataList == null) return;

    OrbModel evokedModel = null;
    bool wasFull = dataList.Count >= orbQueue.Capacity;

    if (wasFull && dataList.Count > 0)
    {
        evokedModel 
= dataList.Last();
        dataList
.RemoveAt(dataList.Count - 1);
    }

    dataList
.Insert(0, orbModel);

    // ========== 2. 表现层：同步 NOrbManager ==========
    var creatureNode = NCombatRoom.Instance?.GetCreatureNode(player.Creature);
    var orbManager = creatureNode?.OrbManager;
    if (orbManager == null) return;

    var nOrbsField = typeof(NOrbManager).GetField("_orbs",
        BindingFlags
.NonPublic | BindingFlags.Instance);
    var nOrbs = nOrbsField?.GetValue(orbManager) as List<NOrb>;

    var containerField = typeof(NOrbManager).GetField("_orbContainer",
        BindingFlags
.NonPublic | BindingFlags.Instance);
    var container = containerField?.GetValue(orbManager) as Control;

    if (nOrbs == null || container == null) return;

    // 2a. 队列已满：尾部球淡出并从列表移除
    if (wasFull && evokedModel != null)
    {
        var lastNOrb = nOrbs.LastOrDefault(n => n.Model == evokedModel);
        if (lastNOrb != null)
        {
            nOrbs
.Remove(lastNOrb);
            var tween = orbManager.CreateTween();
            tween
.TweenProperty(lastNOrb, "modulate:a", 0, 0.25);
            tween
.Chain().TweenCallback(Callable.From(lastNOrb.QueueFreeSafely));
        }
    }
    // 2b. 队列未满：移除一个空槽位
    else
    {
        var emptySlot = nOrbs.FirstOrDefault(n => n.Model == null);
        if (emptySlot != null)
        {
            container
.RemoveChild(emptySlot);
            nOrbs
.Remove(emptySlot);
            emptySlot
.QueueFreeSafely();
        }
    }

    // 2c. 创建新 NOrb
    var newNOrb = NOrb.Create(LocalContext.IsMe(player), orbModel);
    container
.AddChild(newNOrb);
    container
.MoveChild(newNOrb, 0);
    nOrbs
.Insert(0, newNOrb);
    newNOrb
.Position = Vector2.Zero;

    // ========== 3. 触发 evoke ==========
    if (evokedModel != null)
    {
        choiceContext
.PushModel(evokedModel);
        var targets = await evokedModel.Evoke(choiceContext);
        choiceContext
.PopModel(evokedModel);

        if (player.Creature.CombatState != null)
        {
            await Hook.AfterOrbEvoked(choiceContext, player.Creature.CombatState, evokedModel, targets);
            typeof(OrbModel).GetMethod("RemoveInternal",
                BindingFlags
.NonPublic | BindingFlags.Instance)?.Invoke(evokedModel, null);
        }
    }

    // ========== 4. 刷新布局与手柄导航 ==========
    typeof(NOrbManager).GetMethod("TweenLayout",
        BindingFlags
.NonPublic | BindingFlags.Instance)?.Invoke(orbManager, null);
    typeof(NOrbManager).GetMethod("UpdateControllerNavigation",
        BindingFlags
.NonPublic | BindingFlags.Instance)?.Invoke(orbManager, null);
    }


    
}