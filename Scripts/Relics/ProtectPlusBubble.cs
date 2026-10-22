using Haruka.Scripts.Pools;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Commands;
using MinionLib.Minion;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Test.Scripts;

namespace Haruka.Scripts.Relics;
[RegisterRelic(typeof(HarukaRelicPool))]
[RegisterTouchOfOrobasRefinement(typeof(ProtectBubble))]

public sealed class ProtectPlusBubble : ModRelicTemplate

{
    public override RelicRarity Rarity => RelicRarity.Starter;

    
    // 小图标
    public override string PackedIconPath => $"res://Haruka/images/relic/{Id.Entry.ToLowerInvariant()}.png";
    // 轮廓图标
    protected override string PackedIconOutlinePath => $"res://Haruka/images/relic/{Id.Entry.ToLowerInvariant()}.png";
    // 大图标
    protected override string BigIconPath => $"res://Haruka/images/relic/{Id.Entry.ToLowerInvariant()}.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Unpowered)];

    private int TriggerTime = 0;

    public override int DisplayAmount
    {
        get => TriggerTime;
    }

    

    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature.CombatState!.RoundNumber <= 1 && player == Owner)
		{
            Flash();
            await OrbCmd.Channel<BubbleOrb>(choiceContext, player);
        }
    }

    public override async Task AfterOrbEvoked(PlayerChoiceContext choiceContext, OrbModel orb, IEnumerable<Creature> targets)
    {
        if (orb is BubbleOrb bubbleOrb && TriggerTime <= 4)
        {
            TriggerTime++;
            InvokeDisplayAmountChanged();
            await CreatureCmd.Heal(Owner.Creature, 3, true);
        }
        
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        TriggerTime = 0;
        InvokeDisplayAmountChanged();
        return base.AfterCombatEnd(room);
    }


    


    
    
}