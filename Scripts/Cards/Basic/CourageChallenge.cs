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
using MegaCrit.Sts2.Core.Models.CardPools;
using Haruka.Scripts.Cards.Token;




namespace Haruka.Scripts.Cards.Basic
{
    [RegisterCard(typeof(TokenCardPool))]
    public class CourageChallenge : HarukaCard 
    {

        
        private const int energyCost = -1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Token;
        private const TargetType targetType = TargetType.Self;
        

        protected override IEnumerable<DynamicVar> CanonicalVars => [];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(HarukaKeywords.ScareKeyword)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            
        }

        public CourageChallenge() : base(energyCost, type, rarity, targetType)
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
