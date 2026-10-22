using MegaCrit.Sts2.Core.Nodes.Cards;
using HarmonyLib;
using Godot;
using Haruka.Scripts.Cards;
using Haruka.Scripts.Pools;
using MegaCrit.Sts2.Core.Models.CardPools;

[HarmonyPatch(typeof(NCard),"Reload")]
public static class NCardFramePatch
{
    public static float HarukaScale = 440/320f;

    public static float HarukaOffsetY = -10f;
    [HarmonyPostfix]
    public static void Postfix(NCard __instance)
    {
        TextureRect? frame = AccessTools.Field(typeof(NCard), "_frame")?.GetValue(__instance) as TextureRect;
        if (frame == null) return;

        Vector2 scale = frame.Scale;
        bool isHaruka = __instance.Model is HarukaCard &&
                        (__instance.Model.Pool is HarukaCardPool);

        if (isHaruka)
        {
            // 第一次进入时记录原始位置（必须在改 Position 之前）
            if (!frame.HasMeta("orig_pos"))
                frame.SetMeta("orig_pos", frame.Position);

            Vector2 orig = (Vector2)frame.GetMeta("orig_pos");
            frame.Scale = new(HarukaScale, HarukaScale);
            frame.Position = new(orig.X, orig.Y + HarukaOffsetY);
            frame.Material = null;
        }
        else if (scale.X == HarukaScale && scale.Y == HarukaScale)
        {
            frame.Scale = new(1, 1);

            // 用记录的原值还原，而不是写死 0
            if (frame.HasMeta("orig_pos"))
            {
                frame.Position = (Vector2)frame.GetMeta("orig_pos");
                frame.RemoveMeta("orig_pos");
            }
        }


    }
}