using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Combat.Ui.ExtraCornerAmountLabels;
using Test.Scripts;

public sealed class ScenePower : AbstractHarukaPower
{
	private class Data
	{
		public int cardsLeft = 12;
	}

	public event Action? PowerExtraIconAmountLabelsInvalidated;

    public IReadOnlyList<ExtraIconAmountLabelSpec> GetPowerExtraIconAmountLabelSpecs() =>
    [
        ExtraIconAmountLabelSpec.Plain(ExtraIconAmountLabelCorner.BottomRight,
            $"{_baseCardsLeft - GetInternalData<Data>().cardsLeft}")
    ];

	private const int _baseCardsLeft = 12;

	private const string _baseCardsKey = "BaseCards";

	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override int DisplayAmount => GetInternalData<Data>().cardsLeft;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BaseCards", 12m)];

	protected override object InitInternalData()
	{
		return new Data();
	}

	public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
	{
		if (card.Owner != Owner.Player) return;
		
    	Data data = GetInternalData<Data>();
    	data.cardsLeft--;
    	if (data.cardsLeft < 12) return;

    	data.cardsLeft = 0;
    	Flash();
    	await OrbCmd.Channel<BubbleOrb>(new BlockingPlayerChoiceContext(), Owner.Player);
		PowerExtraIconAmountLabelsInvalidated?.Invoke();
	}
}
