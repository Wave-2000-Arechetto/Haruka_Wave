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
using MinionLib.Commands;
using System.Numerics;
using MinionLib.Minion;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.CardSelection;
using STS2RitsuLib;



namespace Haruka.Scripts.Cards.Rare
{
    [RegisterCard(typeof(HarukaCardPool))]
    public class HanabiExiled : HarukaCard
    {
        private const int energyCost = 1;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Rare;
        private const TargetType targetType = TargetType.Self;
        private const bool shouldShowInCardLibrary = true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(4)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromOrb<FireworkOrb>()];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int draw = 0;
        foreach (CardModel item in await CardSelectCmd.FromHand(
            prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, 0, DynamicVars.Cards.IntValue), 
            context: choiceContext, 
            player: base.Owner, 
            filter: null, 
            source: this))
            {
                
                draw++;                
                await CardCmd.Exhaust(choiceContext, item, false, false);
            }

            OrbModel hanabi = ModelDb.Orb<FireworkOrb>().ToMutable();
            if (hanabi is FireworkOrb fireworkOrb && draw > 0)
            {
                fireworkOrb._passiveVal = draw;
                await OrbCmd.Channel(choiceContext, hanabi, Owner);
            }
            
        }   

        public HanabiExiled() : base(energyCost, type, rarity, targetType)
        {
        }


        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars.Cards.UpgradeValueBy(1);
        }
    }
}
