using Godot;
using HarmonyLib;
using Haruka.Scripts.Monster;
using Haruka.Scripts.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Commands;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;

namespace Test.Scripts;

[RegisterOrb]
public class BubbleOrb : ModOrbTemplate
{
    // 被动效果数值，ModifyOrbValue表示是否吃集中等

    private int passiveVal = 1;

    private int evokeVal = 4;
    public override decimal PassiveVal => ModifyOrbValue(passiveVal);

    // 激发效果数值
    public override decimal EvokeVal => evokeVal;

    // 战斗中充能球的文本显示样式。
    // Contextual 表示匹配普通原版充能球行为：平时显示被动值；当充能球被预览为激发时显示激发值。一般用于大多数充能球。
    // SinglePassive 表示只显示被动值。
    // SingleEvoke 表示只显示激发值。
    // Both 表示同时显示被动值和激发值。原版用于黑暗充能球。
    public override ModOrbValueDisplayMode ValueDisplayMode => ModOrbValueDisplayMode.Both;

    // 暗色，使用球的主体色的暗色调
    public override Color DarkenedColor => new(0.1f, 0.2f, 0.5f);

    // 对于图片，只要是godot支持的格式都可以，例如png,jpg,svg等等
    public override OrbAssetProfile AssetProfile => new(
        // 提示文本小图标路径
        IconPath: "res://Haruka/images/orbs/bubble_orb.png",
        // 充能球场景路径
        VisualsScenePath: "res://Haruka/scenes/bubble_orb.tscn"
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
        evokeVal = (int)PassiveVal + evokeVal;
        
    }

    

    // 触发激发，返回受影响的角色
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        PlayEvokeSfx();
        await CreatureCmd.GainBlock(Owner.Creature, EvokeVal,  ValueProp.Unpowered, null);

            

            ICombatState combatState = Owner.Creature.CombatState!;

            
        return [Owner.Creature];
    }


    


    public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if ((int)amount > (int)target.Block
            && dealer != null
            && dealer.Side == CombatSide.Enemy
            && Owner.PlayerCombatState?.OrbQueue.Orbs[0] is BubbleOrb
            && target == Owner.Creature)
        {
            await OrbCmd.EvokeNext(choiceContext, Owner, true);
        }
    }
        
        
}
