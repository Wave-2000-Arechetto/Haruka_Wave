using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Haruka.Scripts.Cards.Token;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Combat.Ui.ExtraCornerAmountLabels;
using Test.Scripts;

public class HimePower : AbstractHarukaPower,
    IPowerExtraIconAmountLabelsProvider,
    IPowerExtraIconAmountLabelsChangeSource
{
    private const int BubbleThreshold = 12;

    public event Action? PowerExtraIconAmountLabelsInvalidated;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => false;
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    // 下面的数字：获得牌的数量
    public override int DisplayAmount => Amount;

    // 上面的数字：本场战斗生成过的浮泡
    public IReadOnlyList<ExtraIconAmountLabelSlot> GetPowerExtraIconAmountLabelSlots()
        => [ExtraIconAmountLabelSlot.At(ExtraIconAmountLabelCorner.TopRight, BubbleCount().ToString())];

    private int BubbleCount()
        => CombatManager.Instance.History.Entries
            .OfType<OrbChanneledEntry>()
            .Count(e => e.Orb.Owner == Owner.Player && e.Orb is BubbleOrb);

    public override Task AfterOrbChanneled(PlayerChoiceContext choiceContext, Player player, OrbModel orb)
    {
        if (orb.Owner == Owner.Player && orb is BubbleOrb)
            PowerExtraIconAmountLabelsInvalidated?.Invoke();
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        PowerExtraIconAmountLabelsInvalidated?.Invoke();

        if (BubbleCount() < BubbleThreshold) return;

        for (int i = 0; i < Amount; i++)
        {
            var returnCard = CombatState.CreateCard<Return>(Owner.Player!);
            await CardPileCmd.Add(returnCard, PileType.Hand, CardPilePosition.Bottom, null);
        }
        await PowerCmd.Remove(this);
    }
}