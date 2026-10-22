using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Orbs;
using Test.Scripts;

public sealed class BubbleNextTurnPower : AbstractHarukaPower
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Channeling),
		HoverTipFactory.FromOrb<BubbleOrb>()
    ];

	
	public override async Task AfterEnergyReset(Player player)
	{
		if (player == base.Owner.Player)
		{
            
            
            await OrbCmd.Channel<BubbleOrb>(new ThrowingPlayerChoiceContext(), base.Owner.Player);
            
			await PowerCmd.Decrement(this);
		}
	}
}