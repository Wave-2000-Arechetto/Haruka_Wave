using HarmonyLib;
using Haruka.Scripts.Cards.Token;
using Haruka.Scripts.Monster;
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


public class MioEnhancePower : AbstractHarukaPower
{
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool AllowNegative => false;

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public bool HasTriggered = false;

    public override async Task AfterDamageReceivedLate(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        var mio = Owner.Pets.FirstOrDefault(c => c.Monster is Mio);
        if (mio != null && target == mio && result.UnblockedDamage > 0)   
        {
            
            if (!HasTriggered)
            {
                CardModel card = Owner.CombatState!.CreateCard<Slash>(Owner.Player!);
                card.SetToFreeThisCombat();
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Player!, CardPilePosition.Bottom);
                await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
                HasTriggered = true;
            }
            
            
            
        }
    }

    public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
    {
        HasTriggered = false;
    }






    
}