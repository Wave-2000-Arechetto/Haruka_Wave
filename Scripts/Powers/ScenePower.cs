using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Test.Scripts;


public sealed class ScenePower : AbstractHarukaPower
{
    private class Data
    {
        public int cardsLeft = BaseCardsLeft;
    }

    private const int BaseCardsLeft = 12;

    private const string BaseCardsKey = "BaseCards";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    // 主数字显示剩余抽牌数（12 → 1），不要再用额外角标叠同一块矩形。
    public override int DisplayAmount => GetInternalData<Data>().cardsLeft;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(BaseCardsKey, BaseCardsLeft)];

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner != Owner.Player) return;

        var data = GetInternalData<Data>();
        data.cardsLeft--;
        InvokeDisplayAmountChanged();

        if (data.cardsLeft > 0) return;

        Flash();
        await OrbCmd.Channel<BubbleOrb>(choiceContext, Owner.Player);

        data.cardsLeft = BaseCardsLeft;
        InvokeDisplayAmountChanged();
    }
}
