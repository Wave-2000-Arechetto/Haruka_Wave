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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;
using Haruka.Scripts.Enums;
using Haruka.Scripts.Cards.Token;




namespace Haruka.Scripts.Cards.Ancient
{
    [RegisterCard(typeof(HarukaCardPool))]
    
    public class FlowerOnHigh : HarukaCard 
    {

        
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Ancient;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(HarukaKeywords.MioKeyword), HoverTipFactory.FromCard<Slash>(IsUpgraded), HoverTipFactory.FromCard<SnowGlow>(IsUpgraded) ];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await SummonMio(choiceContext);
            CardModel slash = CombatState!.CreateCard<Slash>(Owner);
            CardModel snowglow = CombatState!.CreateCard<SnowGlow>(Owner);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(slash);
                CardCmd.Upgrade(snowglow);
            }
            await CardPileCmd.AddGeneratedCardToCombat(slash, PileType.Hand, Owner, CardPilePosition.Bottom);
            await CardPileCmd.AddGeneratedCardToCombat(snowglow, PileType.Hand, Owner, CardPilePosition.Bottom);

            if (!Owner.Creature.HasPower<FlowerOnHighPower>())
            {
                await PowerCmd.Apply<FlowerOnHighPower>(choiceContext, Owner.Creature, 1, Owner.Creature, cardPlay.Card);
            }
        }

        public FlowerOnHigh() : base(energyCost, type, rarity, targetType)
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
