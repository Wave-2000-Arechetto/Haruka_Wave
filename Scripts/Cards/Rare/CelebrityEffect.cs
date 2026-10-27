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
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Models.Powers;



namespace Haruka.Scripts.Cards.Rare
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class CelebrityEffect : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Rare;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<FocusPower>()];

        

        public override IEnumerable<CardKeyword> CanonicalKeywords => [];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var entries = CombatManager.Instance.History.Entries.ToList();
            var orbs = entries.OfType<OrbChanneledEntry>().ToList();
            int count = orbs
        .Where(e => e.Orb.Owner == Owner)
        .Select(e => e.Orb.GetType())
        .Distinct()
        .Count();

            for (int i = 0; i < count; i++)
            {
                await PowerCmd.Apply<CelebrityPower>(choiceContext, Owner.Creature, 1, Owner.Creature, cardPlay.Card);
            }

        }

        

        

        public CelebrityEffect() : base(energyCost, type, rarity, targetType)
        {
        }


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
