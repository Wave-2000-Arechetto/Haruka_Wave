using HarmonyLib;
using Haruka.Scripts.UI;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.CardTargeting;
using STS2RitsuLib.Interop.AutoRegistration;


public class DangerPower : AbstractHarukaPower
{
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override async Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player && (int)EmotionBarManager.Get(Owner.Player) <= -3)
        {
            await EmotionBarManager.Instance!.Set(choiceContext, Owner.Player, 0);
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, Amount, Owner, null);
            
        }
        await PowerCmd.Remove(this);
    }
    
        
    
}