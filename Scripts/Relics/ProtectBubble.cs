using Haruka.Scripts.Enums;
using Haruka.Scripts.Pools;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Test.Scripts;


namespace Haruka.Scripts.Relics;
[RegisterRelic(typeof(HarukaRelicPool))]
[RegisterTouchOfOrobasRefinement(typeof(ProtectPlusBubble))]
public sealed class ProtectBubble : ModRelicTemplate

{
    public override RelicRarity Rarity => RelicRarity.Starter;

    
    // 小图标
    public override string PackedIconPath => $"res://Haruka/images/relic/{Id.Entry.ToLowerInvariant()}.png";
    // 轮廓图标
    protected override string PackedIconOutlinePath => $"res://Haruka/images/relic/{Id.Entry.ToLowerInvariant()}.png";
    // 大图标
    protected override string BigIconPath => $"res://Haruka/images/relic/{Id.Entry.ToLowerInvariant()}.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(HarukaKeywords.BubbleKeyword)];

    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature.CombatState!.RoundNumber <= 1 && player == Owner)
		{
            Flash();
            await OrbCmd.Channel<BubbleOrb>(choiceContext, player);
        }
    }
    
}
