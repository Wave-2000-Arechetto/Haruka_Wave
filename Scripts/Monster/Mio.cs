using Haruka.Scripts.Monster.Intents;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Minion;
using STS2RitsuLib.Scaffolding.Content;
using Godot;
using HarmonyLib;
using Haruka.RitsuAdapters;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using STS2RitsuLib.Interop.AutoRegistration;


namespace Haruka.Scripts.Monster;

[RegisterMonster]
public class Mio : ModMinionTemplate
{
    public override bool HasDeathSfx => true;

    public override bool IsHealthBarVisible => base.Creature.IsAlive;

    

    public override int MinInitialHp => 12;

    public override int MaxInitialHp => 12;



    // ============================================================
    // 位置与大小
    // ============================================================

    public static Vector2 MinOffset => new Vector2(300f, -75f);

    public static Vector2 MaxOffset => new Vector2(300f, -75f);

    public static Vector2 ScaleRange => new Vector2(1f, 2f);


    // ============================================================
    // 资源
    // ============================================================

    public override MonsterAssetProfile AssetProfile =>
        new(
            VisualsScenePath:
                $"res://Haruka/scenes/monsters/{GetType().Name}.tscn");


    public override async Task AfterAddedToRoom()
    {
        
    }

    public override async Task AfterDamageReceivedLate(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == Creature && (int)result.UnblockedDamage > 0)
        {
            await CreatureCmd.TriggerAnim(Creature, "Hurt", 0.0f);
        }
        
    }


    // ============================================================
    // 行为状态机
    // ============================================================

    private MoveState? _defend;
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        List<MonsterState> list = new List<MonsterState>();

        _defend =
            new MoveState(
                "DEFEND",
                async targets => {}
                );

        _defend!.FollowUpState = _defend!;

        list.Add(_defend!);

        return new MonsterMoveStateMachine(
            list,
            _defend!);
    }

    public override async Task OnSummon(
    PlayerChoiceContext choiceContext,
    Player owner,
    MinionSummonOptions options)
{
    await base.OnSummon(choiceContext, owner, options);

    if (_defend != null)
        SetMoveImmediate(_defend!);
}


    // ============================================================
    // 动画
    // ============================================================

    public override CreatureAnimator GenerateAnimator(
        MegaSprite controller)
    {
        AnimState idleState =
            new AnimState(
                "Idle",
                isLooping: true);

        AnimState attackState =
            new AnimState("Attack");

        AnimState dieState =
            new AnimState("Die");

        AnimState hurtState =
            new AnimState("Die");

        AnimState skillState =
            new AnimState("Skill");

        AnimState moveState =
            new AnimState("Move");  

        attackState.NextState = idleState;
        hurtState.NextState = idleState;
        moveState.NextState = idleState;        
        skillState.NextState = idleState;

        CreatureAnimator creatureAnimator =
            new CreatureAnimator(
                idleState,
                controller);

        creatureAnimator.AddAnyState(
            "Attack",
            attackState);

        creatureAnimator.AddAnyState(
            "Dead",
            dieState);

        creatureAnimator.AddAnyState(
            "Hurt",
            hurtState);

        creatureAnimator.AddAnyState(
            "Skill",
            skillState);

        creatureAnimator.AddAnyState(
            "Idle",
            idleState);

        creatureAnimator.AddAnyState(
            "Move",
            moveState);

        return creatureAnimator;
    }






    // ============================================================
    // Mio 保护遥
    // ============================================================


    public override Task AfterModifyingDamageAmount(CardModel? cardSource)
    {
        return base.AfterModifyingDamageAmount(cardSource);
    }



    public decimal _BlockedHpLostBeforeOsty;

    public override decimal ModifyHpLostAfterOsty(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == base.Creature.PetOwner?.Creature
        && base.Creature.IsAlive
        && dealer != null
        && dealer.Side == CombatSide.Enemy)
        {
            if (target.HasPower<MioEnhancePower>())
            {
                if (amount > Creature.CurrentHp)
                {
                    _BlockedHpLostBeforeOsty = Creature.CurrentHp;
                    return amount - Creature.CurrentHp;
                }
                else
                {
                    _BlockedHpLostBeforeOsty = amount;
                    return 0m;
                }
            }
            else
            {
                _BlockedHpLostBeforeOsty = 0.5m * amount;
                return 0.5m * amount;
            }
            
        }
        return base.ModifyHpLostBeforeOsty(target, amount, props, dealer, cardSource);
    }


    public override async Task AfterModifyingHpLostAfterOsty()
    {
        if (_BlockedHpLostBeforeOsty <= 0)
            return;

           
        await CreatureCmd.TriggerAnim(Creature, "Hurt", 0.0f);
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Creature, _BlockedHpLostBeforeOsty, ValueProp.Unpowered, Creature);
        _BlockedHpLostBeforeOsty = 0; 
    }
}