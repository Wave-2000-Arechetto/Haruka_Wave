

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;


namespace Haruka.Scripts.Powers
{
public class FloatPower : AbstractHarukaPower
    {
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Single;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Unpowered),new DynamicVar("DamageDecrease", 0.5m)];
        public override bool AllowNegative => false;

        public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
	{
		if (target != null && target.Side != CombatSide.Player)
		{
			return 1m;
		}
		if (!props.IsPoweredAttack())
		{
			return 1m;
		}
		if (dealer == null)
		{
			return 1m;
		}
		if (dealer != base.Owner)
		{
			return 1m;
		}
		return base.DynamicVars["DamageDecrease"].BaseValue;
	}

        public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	    {
		    if (target == base.Owner && dealer != null && dealer.Side == CombatSide.Player && props.IsPoweredAttack())
		    {
			    Flash();
				await CreatureCmd.Damage(choiceContext, Owner, 10, ValueProp.Unpowered, Owner);
			    await PowerCmd.Apply<VulnerablePower>(choiceContext,Owner, 1, Owner, null, false);
			    await PowerCmd.Remove(this);
		    }
	    }

        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side == CombatSide.Enemy)
		{
			Flash();
            await CreatureCmd.Damage(choiceContext, Owner, 10, ValueProp.Unpowered, base.Owner);
			await PowerCmd.Apply<VulnerablePower>(choiceContext,Owner, 1, Owner, null, false);
			await PowerCmd.Remove(this);
		}
	}

    

        
    }
}