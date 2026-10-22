using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Orbs;

public sealed class GlassNextTurnPower : AbstractHarukaPower
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Channeling),
		HoverTipFactory.FromOrb<GlassOrb>()
    ];

	
	public override async Task AfterEnergyReset(Player player)
	{
		if (player == base.Owner.Player)
		{
            for (int i = 0; i < Amount; i++)
            {
                await OrbCmd.Channel<GlassOrb>(new ThrowingPlayerChoiceContext(), base.Owner.Player);
            }
			await PowerCmd.Remove(this);
		}
	}
}