using HarmonyLib;
using Haruka.Scripts.UI;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.CardTargeting;
using STS2RitsuLib.Interop.AutoRegistration;
using Test.Scripts;


public class InspiredPower : AbstractHarukaPower
{
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Emotion", 0)];

    

    public override decimal ModifyOrbValue(OrbModel orb, decimal value)
	{
		if (base.Owner.Player != orb.Owner)
		{
			return value;
		}

        if (Owner.HasPower<IntoPlayPower>())
        {
            return Math.Max(value + (decimal)base.Amount * (1 + Owner.GetPowerAmount<IntoPlayPower>()), 0m);
        }
		return Math.Max(value + (decimal)base.Amount, 0m);
	}

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (Amount >= 3 && !player.Creature.HasPower<ClimaxPower>())
        {
            return count - 1;
        }
        return base.ModifyHandDraw(player, count);
    }

    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (EmotionBarManager.Instance.Get(player) == 3 && !player.Creature.HasPower<ClimaxPower>())
        {
            await Calm(choiceContext, 3);
        }
    }
    
}