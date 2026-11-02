using Haruka.Scripts.Pools;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

[RegisterRelic(typeof(HarukaRelicPool))]
public sealed class FishStick : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [.. HoverTipFactory.FromEnchantment<Stick>()];

    // 小图标
    public override string PackedIconPath => $"res://Haruka/images/relic/{Id.Entry.ToLowerInvariant()}.png";
    // 轮廓图标
    protected override string PackedIconOutlinePath => $"res://Haruka/images/relic/{Id.Entry.ToLowerInvariant()}.png";
    // 大图标
    protected override string BigIconPath => $"res://Haruka/images/relic/{Id.Entry.ToLowerInvariant()}.png";

    public override async Task AfterObtained()
    {
        foreach (
            CardModel item in await CardSelectCmd.FromDeckForEnchantment(
                prefs: new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, DynamicVars.Cards.IntValue),
                player: Owner,
                enchantment: ModelDb.Enchantment<Stick>(),
                amount: 1
            )
        )
        {
            CardCmd.Enchant<Stick>(item, 1m);
            NCardEnchantVfx? nCardEnchantVfx = NCardEnchantVfx.Create(item);
            if (nCardEnchantVfx != null)
            {
                NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(nCardEnchantVfx);
            }
        }
    }
}