using Haruka.Scripts.Cards;
using Haruka.Scripts.Pools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

using MegaCrit.Sts2.Core.ValueProps;
using Haruka.Scripts.UI;
using MegaCrit.Sts2.Core.HoverTips;
using Haruka.Scripts.Enums;
using MegaCrit.Sts2.Core.Models;
using Haruka.Scripts.Cards.Token;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using Test.Scripts;
using MegaCrit.Sts2.Core.Extensions;



namespace Haruka.Scripts.Cards.Rare
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Froth : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Rare;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        

        protected override IEnumerable<DynamicVar> CanonicalVars => [];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromOrb<BubbleOrb>(), HoverTipFactory.FromOrb<FireworkOrb>(), HoverTipFactory.FromOrb<SpotlightOrb>()];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            for (int i = 0; i < 2; i++)
            {
                List<OrbModel> orbs = new List<OrbModel>();

                orbs.Add(ModelDb.Orb<BubbleOrb>().ToMutable());
                orbs.Add(ModelDb.Orb<FireworkOrb>().ToMutable());
                orbs.Add(ModelDb.Orb<SpotlightOrb>().ToMutable());
                OrbModel? orb = orbs.TakeRandom(1, Owner.RunState.Rng.CombatOrbGeneration).FirstOrDefault();
                if (orb is not null)
                {
                    await OrbCmd.Channel(choiceContext, orb, Owner);
                }
            }

           
            

        }

        public Froth() : base(energyCost, type, rarity, targetType)
        {
        }

        protected override CardLocation GetResultLocationForCardPlay()
        {
            CardLocation result = base.GetResultLocationForCardPlay();
            int orbCount = Owner.PlayerCombatState?.OrbQueue?.Orbs.Count ?? 0;
            if (result.pileType == PileType.Discard && orbCount + 2 > Owner.BaseOrbSlotCount)
            {
                result.pileType = PileType.Draw;
                result.position = CardPilePosition.Random;
            }
            return result;
        }
        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Ethereal);
        }
    }
}
