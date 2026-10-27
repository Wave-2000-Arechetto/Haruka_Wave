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



namespace Haruka.Scripts.Cards.Common
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class Help : HarukaCard
    {
        private const int energyCost = 0;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Common;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        public override bool IsMioCard => true;
        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move), new IntVar("Mio", 3)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(HarukaKeywords.ScareKeyword), HoverTipFactory.FromKeyword(HarukaKeywords.MioKeyword)];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await Scare(choiceContext, 1);
            if (EmotionBarManager.Get(Owner) <= -(int)DynamicVars["Mio"].BaseValue)
            {
                await SummonMio(choiceContext);
            }
        }

        public Help() : base(energyCost, type, rarity, targetType)
        {
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
    ];

    


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["Mio"].UpgradeValueBy(-1);
        }
    }
}
