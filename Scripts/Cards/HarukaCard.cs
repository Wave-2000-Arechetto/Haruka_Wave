
using System.Reflection;
using Godot;
using Haruka.Scripts.Cards;
using Haruka.Scripts.Monster;
using Haruka.Scripts.Pools;
using Haruka.Scripts.UI;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Commands;
using MinionLib.Minion;
using STS2RitsuLib.Scaffolding.Content;


namespace Haruka.Scripts.Cards;

public abstract class HarukaCard : ModCardTemplate

{
    
    public string replaceID(string path)
    {
        return path.Replace("haruka_card_","");
    }


    public virtual bool IsMioCard => false;


    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://Haruka/images/cards/{replaceID(Id.Entry.ToLowerInvariant())}.png",
        // 根据不同类型设置不同卡框
        FramePath: (Type, Pool) switch
        {
            (CardType.Attack, HarukaCardPool) => "res://Haruka/images/bg_attack_512.png",
            (CardType.Power, HarukaCardPool) => "res://Haruka/images/bg_power_512.png",
            (CardType.Skill, HarukaCardPool) => "res://Haruka/images/bg_skill_512.png",
            _ => ""
        },
        PortraitBorderPath: "res://Haruka/images/bg_border.png",
        BannerTexturePath: "res://Haruka/images/bg_banner.png"
    );

    

    public HarukaCard(int baseCost, CardType type, CardRarity rarity, TargetType target) 
    : base(baseCost,type,rarity,target,true)
    {
        
    }

    public HarukaCard(int baseCost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary, bool autoAdd) 
    : base(baseCost,type,rarity,target,showInCardLibrary)
    {
        
    }


    public async Task Scare(PlayerChoiceContext choiceContext, int amount)
    {
        await EmotionBarManager.Instance?.Add(choiceContext, Owner, -amount);
    }

    public async Task Inspire(PlayerChoiceContext choiceContext, int amount)
    {
        await EmotionBarManager.Instance?.Add(choiceContext, Owner, amount);
    }

    public async Task Calm(PlayerChoiceContext choiceContext, int amount)
    {
        if (EmotionBarManager.Instance?.Get(Owner) < 0)
        {
            await Inspire(choiceContext, amount);
        }
        else if (EmotionBarManager.Instance?.Get(Owner) > 0)
        {
            await Scare(choiceContext, amount);
        }
    }

    public static Creature? GetMio(CardModel? card) =>
    card?.Owner.PlayerCombatState?.Pets?
        .FirstOrDefault(p => p.Monster is Mio);

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

// ========== 1.5 写入战斗历史（新增） ==========
if (player.Creature.CombatState != null)
{
    CombatManager.Instance.History.OrbChanneled(
        player.Creature.CombatState, orbModel);
}

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


    public async Task SummonMio(PlayerChoiceContext choiceContext)
    {
        foreach (Creature c in Owner.Creature.CombatState!.Creatures)
        {
            if (c.Monster is Monster.Mio m && m.Creature.PetOwner == Owner && c.IsAlive) 
            {
                await CreatureCmd.Heal(c, c.MaxHp, true);
                return;
            }
        }
        Creature mio = await MinionCmd.AddMinion<Monster.Mio>(choiceContext, Owner, new MinionSummonOptions(12, 10, 6, null, this, MinionPosition.Front));
        await CreatureCmd.TriggerAnim(mio, "Move", 0.0f);
        
        if (Owner.Creature.HasPower<ColdMoonPower>())
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, mio, Owner.Creature.GetPowerAmount<ColdMoonPower>(), Owner.Creature, this);
        }
        if (Owner.Creature.HasPower<MioHpPower>())
        {
            await CreatureCmd.GainMaxHp(mio, Owner.Creature.GetPowerAmount<MioHpPower>());
        }
        
    }

    public async Task MioDamage(PlayerChoiceContext choiceContext, IEnumerable<Creature> targets, decimal damage, ValueProp prop, int hitCount, CardModel cardSource, CardPlay cardPlay)
    {
        Creature? mio = Owner?.Creature?.Pets?.FirstOrDefault(c => c.Monster is Mio);
        if (mio == null) return;
        if (NCombatRoom.Instance?.GetCreatureNode(mio) == null) return;

        if (hitCount > 1)
        {
            await CreatureCmd.TriggerAnim(mio, "Skill", 0.0f);
            for (int i = 0; i < hitCount; i++)
            {
                await Cmd.Wait(0.5f);               
                await CreatureCmd.Damage(choiceContext, targets, damage, prop, mio, cardSource, cardPlay);
            }
        }
        else
        {
            await CreatureCmd.TriggerAnim(mio, "Attack", 0.0f);
            for (int i = 0; i < hitCount; i++)
            {
                await Cmd.Wait(0.5f);
                await CreatureCmd.Damage(choiceContext, targets, damage, prop, mio, cardSource, cardPlay);              
            }            
        }

        if (mio.HasPower<VigorPower>())
        {
            await PowerCmd.Remove<VigorPower>(mio);
        }            
    }

    

    public async Task MioDamage(PlayerChoiceContext choiceContext, Creature target, decimal damage, ValueProp prop, int hitCount, CardModel cardSource, CardPlay cardPlay)
    {
        Creature? mio = Owner?.Creature?.Pets?.FirstOrDefault(c => c.Monster is Mio);
        if (mio == null) return;
        if (NCombatRoom.Instance?.GetCreatureNode(mio) == null) return;

        if (hitCount > 1)
        {
            await CreatureCmd.TriggerAnim(mio, "Skill", 0.0f);
            for (int i = 0; i < hitCount; i++)
            {
                await Cmd.Wait(0.5f);                
                await CreatureCmd.Damage(choiceContext, target, damage, prop, mio, cardSource, cardPlay);              
            }
        }
        else
        {
            await CreatureCmd.TriggerAnim(mio, "Attack", 0.0f);
            for (int i = 0; i < hitCount; i++)
            {
                await Cmd.Wait(0.5f);
                await CreatureCmd.Damage(choiceContext, target, damage, prop, mio, cardSource, cardPlay);
            }
            
        }

        if (mio.HasPower<VigorPower>())
        {
            await PowerCmd.Remove<VigorPower>(mio);
        }    
    }

    //public override async Task BeforeDeath(Creature creature)
    //{
        //if (creature.Monster is Mio)
        //{
            //if (NCombatRoom.Instance?.GetCreatureNode(creature) != null)
                //await CreatureCmd.TriggerAnim(creature, "Die", 0f);
        //}
    //}
}