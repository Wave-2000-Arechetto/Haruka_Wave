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
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Extensions;



namespace Haruka.Scripts.Cards.Rare
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class MatchOfMyriadTricks : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Rare;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        private CardModel? _mockSelectedCard;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(0), new CardsVar(3)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            CardModel? cardModel;
		    if (_mockSelectedCard == null)
		    {
			    List<CardModel> cards = CardFactory.GetDistinctForCombat(base.Owner, base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint), 3, base.Owner.RunState.Rng.CombatCardGeneration).ToList();
			    if (IsUpgraded)
                {
                    foreach (var card in cards)
                    {
                        CardCmd.Upgrade(card);
                    }
                }
                cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: true);
		    }
		    else
		    {
				cardModel = _mockSelectedCard;
		    }
		    if (cardModel != null)
		    {
			    await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner);
		    }

            var hand = PileType.Hand.GetPile(Owner).Cards;
            int max = hand.Count == 0 ? 0 : hand.Max(c => c.EnergyCost.GetWithModifiers(CostModifiers.All));
            var top = hand.Where(c => c.EnergyCost.GetWithModifiers(CostModifiers.All) == max && !c.EnergyCost.CostsX).ToList();
            CardModel? target = top.Count >= 1 ? top.TakeRandom(1, base.Owner.RunState.Rng.CombatCardSelection).FirstOrDefault()! : null;

            if (target != null)
            {
                target.SetToFreeThisCombat();
            }
        }

        public MatchOfMyriadTricks() : base(energyCost, type, rarity, targetType)
        {
        }


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            
        }
    }
}
