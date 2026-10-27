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
using Test.Scripts;



namespace Haruka.Scripts.Cards.Rare
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class TrueMe : HarukaCard
    {
        private const int energyCost = 0;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Rare;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(HarukaKeywords.CalmKeyword)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int emotion = (int)Math.Abs(EmotionBarManager.Get(Owner));
            await Calm(choiceContext, emotion);
            await CardPileCmd.Draw(choiceContext, emotion, Owner);
            await PlayerCmd.GainEnergy(emotion, Owner);
        }

        public TrueMe() : base(energyCost, type, rarity, targetType)
        {
        }


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}
