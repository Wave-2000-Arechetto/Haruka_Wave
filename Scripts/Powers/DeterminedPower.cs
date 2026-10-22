using HarmonyLib;
using Haruka.Scripts.Cards;
using Haruka.Scripts.Cards.Token;
using Haruka.Scripts.UI;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.CardTargeting;
using STS2RitsuLib.Interop.AutoRegistration;


public class DeterminedPower : AbstractHarukaPower
{
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (dealer == null)
        {
            return 0m;
        }
        if (cardSource == null)
        {
            return 0m;
        }
        if (cardSource.Owner != Owner.Player)
        {
            return 0m;
        }
        if (cardSource.Type != CardType.Attack && !(cardSource.Pool is ColorlessCardPool || cardSource.Pool is TokenCardPool))
        {
            return 0m;
        }
        return Amount;
    }
    
        
    
}