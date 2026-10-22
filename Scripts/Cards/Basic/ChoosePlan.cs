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
using Test.Scripts;




namespace Haruka.Scripts.Cards.Basic
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class ChoosePlan : HarukaCard 
    {

        private CardModel? _mockSelectedCard;
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Basic;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Time", 1)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromCard<CourageChallenge>(),HoverTipFactory.FromCard<PerformanceTrain>(), HoverTipFactory.FromKeyword(HarukaKeywords.InspireKeyword), HoverTipFactory.FromKeyword(HarukaKeywords.ScareKeyword)];


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            for (int i = 0; i < DynamicVars["Time"].BaseValue; i++)
            {
                CardModel? cardModel;
                if (_mockSelectedCard == null)
                {
                
                    List<CardModel> cards =
                    [
                        base.CombatState!.CreateCard(ModelDb.Card<CourageChallenge>(),base.Owner), // 直接创建卡牌实例，避免在选择界面显示升级选项
                        base.CombatState!.CreateCard(ModelDb.Card<PerformanceTrain>(),base.Owner),
                    ];
                    cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: false);
                }
                else
                {
                    cardModel = _mockSelectedCard;
                }

                if (cardModel is null)
                {
                    return;
                }

                if (cardModel is CourageChallenge)
                {
                    await Scare(choiceContext, 1);  
                    await OrbCmd.EvokeNext(choiceContext, base.Owner, true);
                    await CardPileCmd.Draw(choiceContext, Owner);
                                   
                }
                else if (cardModel is PerformanceTrain)
                {
                    await Inspire(choiceContext, 1);
                    await OrbCmd.Channel<BubbleOrb>(choiceContext, base.Owner);
                }
            }
        }

        public ChoosePlan() : base(energyCost, type, rarity, targetType)
        {
            
        }

        protected override HashSet<CardTag> CanonicalTags => [
        
            ];


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
