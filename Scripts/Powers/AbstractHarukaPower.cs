using Haruka.Scripts.UI;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Orbs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

[RegisterPower(Inherit = true)]
public abstract class AbstractHarukaPower : ModPowerTemplate
{
    
    public override string? CustomIconPath => $"res://Haruka/images/powers/{replaceID(Id.Entry.ToLowerInvariant())}.png";
    public override string? CustomBigIconPath => $"res://Haruka/images/powers/{replaceID(Id.Entry.ToLowerInvariant())}.png";

    public string replaceID(string path)
    {
        return path.Replace("haruka_power_","");
    }

    public async Task Scare(PlayerChoiceContext choiceContext, int amount)
    {
        await EmotionBarManager.Instance?.Add(choiceContext, Owner.Player!, -amount);
    }

    public async Task Inspire(PlayerChoiceContext choiceContext, int amount)
    {
        await EmotionBarManager.Instance?.Add(choiceContext, Owner.Player!, amount);
    }

    public async Task Calm(PlayerChoiceContext choiceContext, int amount)
    {
        if (EmotionBarManager.Instance?.Get(Owner.Player!) < 0)
        {
            await Inspire(choiceContext, amount);
        }
        else if (EmotionBarManager.Instance?.Get(Owner.Player!) > 0)
        {
            await Scare(choiceContext, amount);
        }
    }

    
}