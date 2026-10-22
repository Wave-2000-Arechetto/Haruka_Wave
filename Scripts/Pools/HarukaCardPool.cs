
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Haruka.Scripts.Pools;


public class HarukaCardPool : TypeListCardPoolModel 
{
    public override string Title => "Haruka";

    // 描述中使用的能量图标。大小为24x24。
    public override string? TextEnergyIconPath => "res://Haruka/images/energy_haruka.png";
    // tooltip和卡牌左上角的能量图标。大小为74x74。
    public override string? BigEnergyIconPath => "res://Haruka/images/energy_haruka_big.png";

    public override string CardFrameMaterialPath => base.CardFrameMaterialPath;

    public override Color DeckEntryCardColor => new(0.97f, 0.78f, 0.05f);

    public override bool IsColorless => false;

    public override string EnergyColorName => "Haruka";

    private static readonly Material? _poolFrameMaterial = MaterialUtils.CreateUnmodulatedHsvShaderMaterial();

    public override Material? PoolFrameMaterial => _poolFrameMaterial;
        
    

    
}