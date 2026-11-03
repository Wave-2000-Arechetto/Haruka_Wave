using HarmonyLib;
using Haruka.Scripts.UI;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.CardTargeting;
using STS2RitsuLib.Interop.AutoRegistration;


public class EntryArrangementsPower : AbstractHarukaPower
{
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    

    protected override IEnumerable<DynamicVar> CanonicalVars => [];


    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
{
    if (side == CombatSide.Player)
        {

            for (int i = 0; i < Amount; i++)
            {
                var hand = Owner?.Player?.PlayerCombatState?.Hand?.Cards;
                if (hand == null) return;

                var candidates = hand
                    .Where(c => c.Type != CardType.Curse && c.Type != CardType.Status)
                    .ToList();

                if (candidates.Count == 0) return;

    
                CardModel? highest = candidates
                    .OrderByDescending(x => x.EnergyCost.GetAmountToSpend())
                    .FirstOrDefault();

                if (highest != null)
                {
                    highest.GiveSingleTurnRetain();
                    highest.EnergyCost.AddUntilPlayed(-1);
                }
            }
            
        

    

        }
}
        
    
}