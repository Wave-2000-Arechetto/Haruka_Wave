using HarmonyLib;
using Haruka.Scripts.Monster;
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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.CardTargeting;
using STS2RitsuLib.Interop.AutoRegistration;


public class ScarePower : AbstractHarukaPower
{
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (Amount > 0 && target != Owner && props.IsPoweredAttack() && (dealer == Owner || (dealer?.Monster is Mio mio && mio.Creature.PetOwner!.Creature == Owner)))
        {
            return Amount * (1 + Owner.GetPowerAmount<IntoPlayPower>());
        }
        if (Amount >= 3 && target == Owner)
        {
            return 2;
        }
        return base.ModifyDamageAdditive(target, amount, props, dealer, cardSource, cardPlay);
    }

    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (EmotionBarManager.Instance.Get(player) == -3 && player.Creature == Owner)
        {
            await Calm(choiceContext, 3);
            await PowerCmd.Apply<StrengthPower>(choiceContext, player.Creature, 1, player.Creature, null);
        }
    }

    
    
}