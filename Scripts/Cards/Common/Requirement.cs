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
using MegaCrit.Sts2.Core.Entities.Creatures;



namespace Haruka.Scripts.Cards.Common
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Requirement : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Common;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new CardsVar(2),
    
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            
    
    await CardPileCmd.Draw(choiceContext,
        (int)DynamicVars["Cards"].BaseValue + Math.Abs((int)EmotionBarManager.Get(Owner)), Owner);
            
        }

        public Requirement() : base(energyCost, type, rarity, targetType)
        {
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

    


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["Cards"].UpgradeValueBy(1); 
        }
    }
}
