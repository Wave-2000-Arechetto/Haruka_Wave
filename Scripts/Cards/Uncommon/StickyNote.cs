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
    public class StickyNote : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Uncommon;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(HarukaKeywords.MioKeyword), ..HoverTipFactory.FromEnchantment<Stick>()];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {

            await SummonMio(choiceContext);
            List<CardModel> cardsIn = (from c in PileType.Draw.GetPile(base.Owner).Cards
			orderby c.Rarity, c.Id
			select c).ToList();

            List<CardModel> no_enchantment_list = cardsIn.Where(c => c.Enchantment == null).ToList();

		    List<CardModel> list = (await CardSelectCmd.FromSimpleGrid(
                choiceContext, no_enchantment_list, base.Owner, 
                new CardSelectorPrefs(
                    CardSelectorPrefs.TransformSelectionPrompt, 
                    base.DynamicVars.Cards.IntValue
                    ))).ToList();
            if (list.Count == 0)
            {
                return;
            }

            foreach (CardModel c in list)
            {
                CardCmd.Enchant<Stick>(c,1);
                
            }
        }

        public StickyNote() : base(energyCost, type, rarity, targetType)
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