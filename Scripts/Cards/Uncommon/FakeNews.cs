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
using MegaCrit.Sts2.Core.Models.Orbs;
using Haruka.Scripts.Enums;
using Haruka.Scripts.Cards.Token;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.CardSelection;



namespace Haruka.Scripts.Cards.Uncommon
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class FakeNews : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(HarukaKeywords.MioKeyword), HoverTipFactory.FromCard<Slash>(IsUpgraded)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {

            await SummonMio(choiceContext);
            List<CardModel> cardModels = (await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(SelectionScreenPrompt, 0, 2),
                context: choiceContext,
                player: Owner,
                filter: null,
                source: this)).ToList();

            if (cardModels.Count == 0)
            {
                return;
            }

            foreach (CardModel c in cardModels)
            {
                CardModel slash = Owner.Creature.CombatState!.CreateCard<Slash>(Owner);
                if (IsUpgraded)
                {
                    CardCmd.Upgrade(slash);
                }
                await CardCmd.Transform(c, slash);
                
            }
        }

        public FakeNews() : base(energyCost, type, rarity, targetType)
        {
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

   


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            
        }
    }
}