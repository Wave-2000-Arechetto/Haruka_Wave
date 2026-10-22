

using Haruka.Scripts.Pools;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using Haruka.Scripts.Relics;
using Haruka.Scripts.Cards.Basic;
using MegaCrit.Sts2.Core.Combat;

using System.Dynamic;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Interop.AutoRegistration;
using Haruka.Scripts.Data;
using STS2RitsuLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Entities.Cards;
using Haruka.Scripts.UI;


namespace Haruka.Scripts.Characters;

[RegisterCharacter]
public class Haruka : ModCharacterTemplate<HarukaCardPool, HarukaRelicPool, HarukaPotionPool>
{

    public static int selectedSkinIndex;
    public override Color NameColor => new(0.42f,0.42f,1f);

    public override bool RequiresEpochAndTimeline => false;

    
    public override int BaseOrbSlotCount => 3;


    public override float AttackAnimDelay => 0.2f;
    public override float CastAnimDelay => 0.2f;


    public override Color EnergyLabelOutlineColor => new(0.50f, 0.35f, 0.12f);

    public override Color MapDrawingColor => new(0.50f, 0.35f, 0.12f);

    public override CharacterGender Gender => CharacterGender.Feminine;

    public override int StartingHp => 64;

    public override int StartingGold => 99;

    

    

    // 定义可用皮肤
    public static readonly List<HarukaCharacterSkinData> Skins = new()
    {
        new("default", "默认", "res://Haruka/scenes/HarukaModel.tscn"),
        new("illusion",  "常世之幻", "res://Haruka/scenes/HarukaIllusionModel.tscn"),
        
    };

    // 运行时当前皮肤（从存档恢复）
    public static HarukaCharacterSkinData SelectedSkin { get; set; } = Skins[0];

    // 角色模型路径改为读取当前皮肤
    

// 提取我们在前面用 "mod_progress" 这个 Key 注册时的数据
    

    // 人物模型tscn路径。要自定义见下。
    public override string CustomVisualsPath
    {
        
        get
        {
            switch (RitsuLibFramework.GetDataStore("Haruka").Get<SkinData>("skin_data").SelectedSkinIndex)
            {
                case 0:
                return "res://Haruka/scenes/HarukaModel.tscn";
                
                case 1:
                return "res://Haruka/scenes/HarukaIllusionModel.tscn";

                
            }
        
            return "res://Haruka/scenes/HarukaModel.tscn";
        }
    }

    
    // 卡牌拖尾路径。
    // public override string CustomTrailPath => "res://scenes/vfx/card_trail_ironclad.tscn";
    // 人物头像路径。
    public override string CustomIconTexturePath => "res://haruka.png";
    // 人物头像2号。
    public override string CustomIconPath => "res://Haruka/scenes/Haruka.tscn";
    // 能量表盘tscn路径。要自定义见下。
    public override string CustomEnergyCounterPath => "res://Haruka/scenes/EnergyPanel.tscn";
    // 篝火休息动画。
    public override string CustomRestSiteAnimPath
    {
        get
        {
            switch (RitsuLibFramework.GetDataStore("Haruka").Get<SkinData>("skin_data").SelectedSkinIndex)
            {
                case 0:
                return "res://Haruka/scenes/HarukaRestModel.tscn";
                
                case 1:
                return "res://Haruka/scenes/HarukaIllusionRestModel.tscn";

                
            }
        
            return "res://Haruka/scenes/HarukaRestModel.tscn";
        }
    }
    // 商店人物动画。
    public override string CustomMerchantAnimPath
    {
        get
        {
            switch (RitsuLibFramework.GetDataStore("Haruka").Get<SkinData>("skin_data").SelectedSkinIndex)
            {
                case 0:
                return "res://Haruka/scenes/HarukaMerchantModel.tscn";
                
                case 1:
                return "res://Haruka/scenes/HarukaIllusionMerchantModel.tscn";

                
            }
        
            return "res://Haruka/scenes/HarukaMerchantModel.tscn";
        }
    }

    public string CustomPaperAnimPath
    {
        get
        {
            switch (RitsuLibFramework.GetDataStore("Haruka").Get<SkinData>("skin_data").SelectedSkinIndex)
            {
                case 0:
                return "res://Haruka/images/hands/multiplayer_hand_Haruka_paper.png";
                
                case 1:
                return "res://Haruka/images/hands/multiplayer_hand_Haruka_paper_illusion.png";

                
            }
        
            return "res://Haruka/images/hands/multiplayer_hand_Haruka_paper.png";
        }
    }

    public string CustomScissorsAnimPath
    {
        get
        {
            switch (RitsuLibFramework.GetDataStore("Haruka").Get<SkinData>("skin_data").SelectedSkinIndex)
            {
                case 0:
                return "res://Haruka/images/hands/multiplayer_hand_Haruka_scissors.png";
                
                case 1:
                return "res://Haruka/images/hands/multiplayer_hand_Haruka_scissors_illusion.png";

                
            }
        
            return "res://Haruka/images/hands/multiplayer_hand_Haruka_scissors.png";
        }
    }

    public string CustomRockAnimPath
    {
        get
        {
            switch (RitsuLibFramework.GetDataStore("Haruka").Get<SkinData>("skin_data").SelectedSkinIndex)
            {
                case 0:
                return "res://Haruka/images/hands/multiplayer_hand_Haruka_rock.png";
                
                case 1:
                return "res://Haruka/images/hands/multiplayer_hand_Haruka_rock_illusion.png";

                
            }
        
            return "res://Haruka/images/hands/multiplayer_hand_Haruka_rock.png";
        }
    }

    public string CustomPointAnimPath
    {
        get
        {
            switch (RitsuLibFramework.GetDataStore("Haruka").Get<SkinData>("skin_data").SelectedSkinIndex)
            {
                case 0:
                return "res://Haruka/images/hands/multiplayer_hand_Haruka_point.png";
                
                case 1:
                return "res://Haruka/images/hands/multiplayer_hand_Haruka_point_illusion.png";

                
            }
        
            return "res://Haruka/images/hands/multiplayer_hand_Haruka_point.png";
        }
    }
    

    // 人物选择背景。
    public override string CustomCharacterSelectBgPath => "res://Haruka/scenes/HarukaSelectBg.tscn";
    // 人物选择图标。
    public override string CustomCharacterSelectIconPath => "res://Haruka/images/icon/char_select_haruka.png";
    // 人物选择图标-锁定状态。
    public override string CustomCharacterSelectLockedIconPath => "res://Haruka/images/icon/char_select_haruka_locked.png";
    // 人物选择过渡动画。
    //public override string CustomCharacterSelectTransitionPath => "res://Haruka.png";
    // 地图上的角色标记图标、表情轮盘上的角色头像
    // public override string CustomMapMarkerPath => null;
    // 攻击音效
    public override string CustomAttackSfx => "res://Haruka/audio/p_atk_archet_s.wav";
    // 施法音效
    // public override string CustomCastSfx => null;
    // 死亡音效
    // public override string CustomDeathSfx => null;
    // 角色选择音效
    // public override string CharacterSelectSfx => null;
    // 过渡音效。这个不能删。
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

    public override CharacterAssetProfile AssetProfile => CharacterAssetProfiles.Merge(
        CharacterAssetProfiles.Ironclad(),
        new(
            Scenes: new(
                // 人物模型tscn路径。
                VisualsPath: CustomVisualsPath,
                // 能量表盘tscn路径。
                EnergyCounterPath: "res://Haruka/scenes/EnergyPanel.tscn",
                // 商店人物场景。
                MerchantAnimPath: CustomMerchantAnimPath,
                // 篝火休息场景。
                RestSiteAnimPath: CustomRestSiteAnimPath
            ),
            Ui: new(
                // 人物头像路径。
                IconTexturePath: "res://Haruka.png",
                // 人物头像2号。
                IconPath: "res://Haruka/scenes/Haruka.tscn",
                // 人物选择背景。
                CharacterSelectBgPath: "res://Haruka/scenes/HarukaSelectBg.tscn",
                // 人物选择图标。
                CharacterSelectIconPath: "res://Haruka/images/icon/char_select_haruka.png",
                // 人物选择图标-锁定状态。
                CharacterSelectLockedIconPath: "res://Haruka/images/icon/char_select_haruka_locked.png"
                // 人物选择过渡动画。
                // CharacterSelectTransitionPath: "res://materials/transitions/ironclad_transition_mat.tres",
                // 地图上的角色标记图标、表情轮盘上的角色头像
                // MapMarkerPath: null
            ),
            Vfx: new(
                // 卡牌拖尾场景。
                // TrailPath: "res://scenes/vfx/card_trail_ironclad.tscn"
            ),
            Audio: new(
                // 攻击音效
                AttackSfx: null,
                // 施法音效
                CastSfx: null,
                // 死亡音效
                DeathSfx: null,
                // 角色选择音效
                CharacterSelectSfx: null,
                // 过渡音效
                CharacterTransitionSfx: "event:/sfx/ui/wipe_ironclad"
            ),
            Multiplayer: new(
                // 多人模式-手指。
                ArmPointingTexturePath: CustomPointAnimPath,
                // 多人模式剪刀石头布-石头。
                ArmRockTexturePath: CustomRockAnimPath,
                // 多人模式剪刀石头布-布。
                ArmPaperTexturePath: CustomPaperAnimPath,
                // 多人模式剪刀石头布-剪刀。
                ArmScissorsTexturePath: CustomScissorsAnimPath
            )));
    
    public override List<string> GetArchitectAttackVfx() => [
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
    ];

    


    
    protected override IEnumerable<StartingDeckEntry> StartingDeckEntries => [
        new(typeof(StrikeHaruka), 4),
        new(typeof(DefendHaruka), 4),
        new(typeof(ChoosePlan), 1),
        new(typeof(Broker), 1),
        
    ];

    protected override IEnumerable<Type> StartingRelicTypes => [
        typeof(ProtectBubble)
        
    ];

    protected override CreatureAnimator? SetupCustomCreatureAnimator(MegaSprite controller)
{
    // 设定动画名和是否循环播放
    AnimState animState = new("idle_loop", isLooping: true);
    AnimState animState2 = new("cast");
    AnimState animState3 = new("attack");
    AnimState animState4 = new("die");
    AnimState state = new("die");
    AnimState animState5 = new("Skill_3"); // new
    AnimState animState6 = new("relaxed", isLooping: true);

    // 设定播放后自动跳转，例如这里都是返回idle
    animState2.NextState = animState;
    animState3.NextState = animState;
    animState4.NextState = animState;
    animState5.NextState = animState;
    animState6.AddBranch("idle_loop", animState);

    // 绑定播放动画名
    CreatureAnimator creatureAnimator = new(animState, controller);
    creatureAnimator.AddAnyState("idle_loop", animState);
    creatureAnimator.AddAnyState("Dead", state);
    creatureAnimator.AddAnyState("Hit", animState4);
    creatureAnimator.AddAnyState("Attack", animState3);
    creatureAnimator.AddAnyState("Cast", animState2);
    creatureAnimator.AddAnyState("Skill_3", animState5); // new
    creatureAnimator.AddAnyState("Relaxed", animState6);
    return creatureAnimator;
}

}