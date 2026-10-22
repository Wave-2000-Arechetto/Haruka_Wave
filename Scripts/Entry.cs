using System.Reflection;
using Godot.Bridge;
using HarmonyLib;
using Haruka.Scripts.Cards.Ancient;
using Haruka.Scripts.Cards.Basic;
using Haruka.Scripts.Data;
using Haruka.Scripts.Patch;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MinionLib.Layout;
using STS2RitsuLib;
using STS2RitsuLib.Interop;
using STS2RitsuLib.Utils.Persistence;

namespace Haruka.Scripts;

// 必须要加的属性，用于注册Mod。字符串和初始化函数命名一致。
[ModInitializer("Init")]
public class Entry
{

    private const string SkinPath = "res://Haruka/char/default/haruka_spine_skeleton_data_resource.tres";
    // Harmony的ID，确保唯一性
    private const string HarmonyId = "sts2.wave.Haruka.spineskintemplate";
    private static Harmony? _harmony;
    // 初始化函数
    public static void Init()
	{
		var assembly = Assembly.GetExecutingAssembly();
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly);
        ModTypeDiscoveryHub.RegisterModAssembly("HARUKA", assembly);
        RitsuLibFramework.RegisterArchaicToothTranscendenceMapping<Broker, FlowerOnHigh>();
		
		//RitsuLibFramework.CreateContentPack("Archetto")
		//.CardLibraryCompendiumSharedPoolFilter<TrackPool>(
		//	"track",
		//	"res://track.png"
		//)
		//.Apply();

		using (RitsuLibFramework.BeginModDataRegistration("Haruka"))
{
    var store = RitsuLibFramework.GetDataStore("Haruka");

    // 向磁盘注册一项数据
    store.Register<SkinData>(
        key: "skin_data",               // ID键值，一旦确定不要轻易改动
        fileName: "skin_data.json", // 决定它在硬盘里的名字
        scope: SaveScope.Global,           // 这是一份通用的全局数据
        defaultFactory: () => new SkinData(), // 首次创建的默认值
        autoCreateIfMissing: true          // 是否在玩家第一次挂载 Mod 后自动帮他在硬盘里生成文件
    );

    MinionLayoutManager.Register(new MioLayout(), -1);
};

        

        

        

        


		
		_harmony = new Harmony("sts2.wave.Haruka");
		_harmony.PatchAll();
		
		Log.Debug("Haruka Mod initialized!");
	}
}
