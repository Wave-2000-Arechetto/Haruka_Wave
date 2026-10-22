using HarmonyLib;
using Haruka.Scripts.Cards.Uncommon;
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
using STS2RitsuLib.Combat.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

[RegisterPower(Inherit = true)]
public class ChannelEffectPower : ModTemporaryAppliedPowerTemplate<ChannelEffect, StrengthPower>
{

    public override string? CustomIconPath => $"res://Haruka/images/powers/{replaceID(Id.Entry.ToLowerInvariant())}.png";
    public override string? CustomBigIconPath => $"res://Haruka/images/powers/{replaceID(Id.Entry.ToLowerInvariant())}.png";

    public string replaceID(string path)
    {
        return path.Replace("haruka_power_","");
    }
    

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player)
        {
            await EmotionBarManager.Instance!.Set(choiceContext, Owner.Player!, Amount);
        }
    }

        
    
}