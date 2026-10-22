using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Haruka.Scripts.Powers;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace Haruka.Scripts.Monster.Intents;

public class MioDamageIntent : AttackIntent
{
    public override int Repeats => 1;

    protected override string IntentPrefix =>
        "HARUKA_INTENT_MIO_DAMAGE";

    protected override string SpritePath =>
        "res://Haruka/scenes/MioDamageIntent.tres";

    public override IEnumerable<string> AssetPaths => [SpritePath];

    public override Texture2D GetTexture(
    IEnumerable<Creature> targets,
    Creature owner)
{
    return PreloadManager.Cache.GetTexture2D(
        "res://Haruka/scenes/MioDamageIntent.tres");
}

    public MioDamageIntent()
    {
        base.DamageCalc = () => 0;
    }

    public override int GetTotalDamage(
        IEnumerable<Creature> targets,
        Creature owner)
    {
        Creature? haruka = owner.PetOwner?.Creature;

        if (haruka == null || haruka.CombatState == null)
        {
            return 0;
        }

        int totalDamage = 0;

        foreach (Creature enemy in haruka.CombatState.HittableEnemies)
        {
            if (!enemy.IsAlive || enemy.Monster == null)
            {
                continue;
            }

            foreach (AttackIntent intent in enemy.Monster.NextMove.Intents
                .OfType<AttackIntent>())
            {
                int damagePerHit = GetOriginalDamage(intent);

                if (damagePerHit <= 0)
                {
                    continue;
                }

                int repeat = Math.Max(1, intent.Repeats);

                totalDamage += GetMioDamage(
                    damagePerHit,
                    haruka) * repeat;
            }
        }

        return totalDamage;
    }

    public override LocString GetIntentLabel(
        IEnumerable<Creature> targets,
        Creature owner)
    {
        Creature? haruka = owner.PetOwner?.Creature;

        LocString label = new LocString(
            "intents",
            "FORMAT_DAMAGE_SINGLE");

        if (haruka == null || haruka.CombatState == null)
        {
            label.Add("Damage", "0");
            return label;
        }

        string damageText = GetMioDamageText(haruka);

        label.Add("Damage", damageText);

        return label;
    }

    private static int GetOriginalDamage(
        AttackIntent intent)
    {
        if (intent.DamageCalc == null)
        {
            return 0;
        }

        decimal damage = intent.DamageCalc();

        return Math.Max(0, (int)damage);
    }

    private static int GetMioDamage(
        int damage,
        Creature haruka)
    {
        if (damage <= 0)
        {
            return 0;
        }

        decimal targetRatio;

        if (haruka.HasPower<MioEnhancePower>())
        {
            // 强化 Mio：
            // 遥承担 30%
            // Mio 承担 70%
            targetRatio = 0.3m;
        }
        else
        {
            // 普通 Mio：
            // 遥承担 70%
            // Mio 承担 30%
            targetRatio = 0.7m;
        }

        int harukaDamage =
            (int)Math.Ceiling(damage * targetRatio);

        harukaDamage =
            Math.Min(harukaDamage, damage);

        return damage - harukaDamage;
    }

    private static string GetMioDamageText(
        Creature haruka)
    {
        List<string> damageParts = [];

        foreach (Creature enemy in haruka.CombatState.HittableEnemies)
        {
            if (!enemy.IsAlive || enemy.Monster == null)
            {
                continue;
            }

            foreach (AttackIntent intent in enemy.Monster.NextMove.Intents
                .OfType<AttackIntent>())
            {
                int damagePerHit =
                    GetOriginalDamage(intent);

                int repeat =
                    Math.Max(1, intent.Repeats);

                if (damagePerHit <= 0)
                {
                    continue;
                }

                int mioDamagePerHit =
                    GetMioDamage(
                        damagePerHit,
                        haruka);

                if (mioDamagePerHit <= 0)
                {
                    continue;
                }

                if (repeat > 1)
                {
                    damageParts.Add(
                        $"{mioDamagePerHit}×{repeat}");
                }
                else
                {
                    damageParts.Add(
                        mioDamagePerHit.ToString());
                }
            }
        }

        if (damageParts.Count == 0)
        {
            return "0";
        }

        return string.Join("+", damageParts);
    }
}