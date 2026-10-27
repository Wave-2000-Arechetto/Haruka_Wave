using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using Test.Scripts;
using HarmonyLib;

namespace Haruka.Scripts.UI
{
	public partial class EmotionBarManager : Node
	{
		public static EmotionBarManager? Instance { get; private set; }
	// 新增：本场战斗进入恐惧的次数
		public readonly Dictionary<Player, float> _ScareEnterCounts = new();

		private readonly Dictionary<Player, float> _values = new();
		public const float MIN = -3f;
		public const float MAX = 3f;

		public event Action<Player, float>? OnValueChanged;

		public override void _EnterTree()
		{
			Instance = this;
			Instance._ScareEnterCounts.Clear();
		}

		public int GetScareEnterCount(Player owner)
		{
			return (int)_ScareEnterCounts.GetValueOrDefault(owner, 0);
		}

		private void IncrementScareEnterCount(Player owner)
		{
			_ScareEnterCounts[owner] = _ScareEnterCounts.GetValueOrDefault(owner, 0) + 1;   
		}

		public async Task Add(PlayerChoiceContext choiceContext, Player owner, float amount)
		{
			if (owner == null) return;

			float current = _values.GetValueOrDefault(owner, 0f);
			float next = Mathf.Clamp(current + amount, MIN, MAX);

			// 过零触发：所有 orb 生成按顺序 await 执行
			if (CrossedZero(current, next))
			{
				if (owner.Creature.HasPower<UpDownPower>())
				{
					await TriggerUpDownOrbs(choiceContext, owner);
				}

				if (next < 0)
				{
					IncrementScareEnterCount(owner);
				}
			}

			await ResetEmotion(choiceContext, owner, next);
			_values[owner] = next;

			OnValueChanged?.Invoke(owner, next);
		}

		public async Task Set(PlayerChoiceContext choiceContext, Player owner, float amount)
		{
			if (owner == null) return;

			float current = _values.GetValueOrDefault(owner, 0f);
			float next = Mathf.Clamp(amount, MIN, MAX);

			if (CrossedZero(current, next))
			{
				if (owner.Creature.HasPower<UpDownPower>())
				{
					await TriggerUpDownOrbs(choiceContext, owner);
				}

				if (next < 0)
				{
					IncrementScareEnterCount(owner);
				}
			}

			_values[owner] = next;
			await ResetEmotion(choiceContext, owner, next);

			OnValueChanged?.Invoke(owner, next);
		}

		public static float Get(Player owner)
		{
			if (owner == null) return 0f;
			return Instance?._values.GetValueOrDefault(owner, 0f) ?? 0f;
		}

		public async Task Reset(PlayerChoiceContext choiceContext, Player owner)
		{
			if (owner == null) return;

			await ResetEmotion(choiceContext, owner, 0f);
			_values[owner] = 0f;

			OnValueChanged?.Invoke(owner, 0f);
		}

		private static bool CrossedZero(float current, float next)
		{
			return (current < 0 && next >= 0)
				|| (current > 0 && next <= 0)
				|| (current == 0 && next != 0);
		}

		private static async Task TriggerUpDownOrbs(PlayerChoiceContext choiceContext, Player owner)
		{
			int amount = (int)owner.Creature.GetPower<UpDownPower>()!.Amount;
			for (int i = 0; i < amount; i++)
			{
				OrbModel orb = OrbModel.GetRandomOrb(owner.RunState.Rng.CombatOrbGeneration).ToMutable();
				await OrbCmd.Channel(choiceContext, orb, owner);
			}
		}

		public async Task ResetEmotion(PlayerChoiceContext choiceContext, Player owner, float next)
		{
			// 先移除旧的情感 power（逐个 await，保证移除顺序确定）
			foreach (PowerModel p in owner.Creature.Powers.ToList())
			{
				if (p is CalmPower || p is ScarePower || p is InspiredPower)
				{
					await PowerCmd.Remove(p);
				}
			}

			if (next < 0)
			{
				await PowerCmd.Apply<ScarePower>(
					choiceContext, owner.Creature,
					(int)Math.Abs(next), owner.Creature, null);
			}
			if (next == 0)
			{
				await PowerCmd.Apply<CalmPower>(
					choiceContext, owner.Creature, 1, owner.Creature, null);

				if (owner.Creature.HasPower<JoyfulPower>())
				{
					await CardPileCmd.Draw(
						choiceContext,
						owner.Creature.GetPowerAmount<JoyfulPower>(), owner);
				}
				if (owner.Creature.HasPower<HandballPower>())
				{
					for (int i = 0; i < owner.Creature.GetPowerAmount<HandballPower>(); i++)
					{
						await OrbCmd.Channel<BubbleOrb>(choiceContext, owner);
					}
				}
			}
			if (next > 0)
			{
				await PowerCmd.Apply<InspiredPower>(
					choiceContext, owner.Creature,
					(int)next, owner.Creature, null);
			}
		}

		[HarmonyPatch(typeof(Hook), "AfterPlayerTurnStart")]
		public static class OnCardClickedPatch
		{
			[HarmonyPostfix]
			public static void Postfix(PlayerChoiceContext choiceContext, Player player)
			{
				if (player.Creature.CombatState.RoundNumber == 1)
				{
					if (Instance != null)
					{
						Instance._ScareEnterCounts[player] = 0;
					}
				}
			}
		}

	}    
}
