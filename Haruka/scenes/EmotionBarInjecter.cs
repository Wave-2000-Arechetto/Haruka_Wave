using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Entities.Players;
using Haruka.Scripts.UI;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using Haruka.Scripts.Cards;

namespace Haruka.Scripts.Patches
{
    [HarmonyPatch(typeof(Hook))]
    [HarmonyPatch(nameof(Hook.AfterPlayerTurnStart))]
    public static class EmotionBarInjector
    {

        
        static void Postfix(PlayerChoiceContext choiceContext, Player player)
        {

            if (player == null) return;

            if (player.Creature?.CombatState?.RoundNumber != 1) return;

            if (!player.Deck.Cards.Any(x => x is HarukaCard)) return;

            PowerCmd.Apply<CalmPower>(choiceContext, player.Creature, 1, player.Creature, null);

            if (EmotionBarManager.Instance == null)
            {
                var creatureNode = player.Creature?.GetCreatureNode();
                if (creatureNode == null) return;

                creatureNode.AddChild(new EmotionBarManager());
            }

            // 工厂创建 + 挂载（像 SanktaBulletBar.Create + AddChild）
            var bar = EmotionBar.Create(player);
            bar.Name = "HarukaEmotionBar";
            bar.AttachToPlayer(); // 内部处理挂到玩家节点还是 Fallback
        

            // 初始化显示
            EmotionBarManager.Instance!.Set(choiceContext, player, 0);
            bar.SetValue(EmotionBarManager.Instance!.Get(player));
        }

        
    }
}