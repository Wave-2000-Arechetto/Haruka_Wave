using Godot;
using System.Collections.Generic;
using System.Linq;

using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Random;
using STS2RitsuLib.Utils;

using STS2RitsuLib;
using Haruka.Scripts.Data;

namespace Haruka.Scripts;

public partial class HarukaSkinPreviewPanel : Control
{
	[Export] private SubViewport _modelViewport = null!;
	[Export] private Label _skinNameLabel = null!;
	[Export] private TextureButton _leftBtn = null!;
	[Export] private TextureButton _rightBtn = null!;
	[Export] private Button _confirmBtn = null!;

	[Export] private Node2D _SpineSprite1 = null!;
	[Export] private Node2D _SpineSprite2 = null!;
	[Export] private Node2D _SpineSprite3 = null!;

	private List<HarukaCharacterSkinData> _skins = new();
	public static int _currentIndex;
	private Node? _currentModel;

	public override void _Ready()
{
	// 按节点名递归查找，支持任意嵌套层级
	_modelViewport = FindChild("SubViewport", true) as SubViewport;
	_skinNameLabel = FindChild("HarukaSkinNameLabel", true) as Label;
	_leftBtn = FindChild("HarukaLeftBtn", true) as TextureButton;
	_rightBtn = FindChild("HarukaRightBtn", true) as TextureButton;
	_SpineSprite1 = FindChild("HarukaModel", true) as Node2D;
	_SpineSprite2 = FindChild("HarukaIllusionModel", true) as Node2D;
	
	
	
	
	 // 可选，没有就不崩

	// 安全检查
	if (_modelViewport == null) { GD.PushError("[SkinPreviewPanel] 找不到 SubViewport"); return; }
	if (_skinNameLabel == null) { GD.PushError("[SkinPreviewPanel] 找不到 SkinNameLabel"); return; }
	if (_leftBtn == null) { GD.PushError("[SkinPreviewPanel] 找不到 LeftBtn"); return; }
	if (_rightBtn == null) { GD.PushError("[SkinPreviewPanel] 找不到 RightBtn"); return; }
	

	_skins = Scripts.Characters.Haruka.Skins;
	_currentIndex = RitsuLibFramework.GetDataStore("Haruka").Get<SkinData>("skin_data").SelectedSkinIndex;
	if (_currentIndex < 0) _currentIndex = 0;

	_leftBtn.Pressed += () => ChangeSkin(-1);
	_rightBtn.Pressed += () => ChangeSkin(1);
	

	Refresh();
}

	

	private void ChangeSkin(int delta)
	{
		_currentIndex = (_currentIndex + delta + _skins.Count) % _skins.Count;
		Scripts.Characters.Haruka.selectedSkinIndex = _currentIndex;
		var store = RitsuLibFramework.GetDataStore("Haruka");

		// 使用内置的 Modify 闭包函数进行修改，它能确保状态的线程一致性
		store.Modify<SkinData>("skin_data", data => 
		{
			data.SelectedSkinIndex = _currentIndex;
	
		});

// 你必须手动调用，否则不会保存。
		store.Save("skin_data");
		Refresh();
	}

	private void Refresh()
	{
		var skin = _skins[_currentIndex];
		_skinNameLabel.Text = skin.Name;
		_SpineSprite1.Visible = false;
		_SpineSprite2.Visible = false;
		
		
		

		if (_currentIndex == 0)
		{
			_SpineSprite1.Visible = true;
			_SpineSprite2.Visible = false;
			
		}
		if (_currentIndex == 1)
		{
			_SpineSprite2.Visible = true;
			_SpineSprite1.Visible = false;
			
		}
		

		if (_currentIndex == 0)
		{
			TryPlayIdle(_SpineSprite1);
		}
		if (_currentIndex == 1)
		{
			TryPlayIdle(_SpineSprite2);
		}
		
		
	}

	

	private void TryPlayIdle(Node model)
	{
		foreach (var spine in model.GetChildren(true).OfType<Node2D>().Where(n => n.GetClass() == "SpineSprite"))
		{
			var animState = new MegaSprite(spine).GetAnimationState();
			// Set the animation; API may not return the track entry directly
			animState.SetAnimation("idle_loop");
			// Try to get the current track entry at track 0 and randomize its time if available
			var trackEntry = animState.GetCurrent(0);
			trackEntry?.SetTrackTime(trackEntry.GetAnimationEnd() * Rng.Chaotic.NextFloat());
		}
	}

	

	

	

}
