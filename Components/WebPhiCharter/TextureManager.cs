using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection;

namespace yt6983138.github.io.Components.WebPhiCharter;
public static class TextureManager
{
	/// <summary>
	/// multiply canvas width with this bam u get line width
	/// </summary>
	public const float LineBaseLength = 1.7777777777777777777f;
	/// <summary>
	/// multiply canvas width with this bam u get note width
	/// </summary>
	public const float NoteBaseScale = 1 / 7.2f;

	public static (float Width, float Height) TapSize { get; set; }
	public static (float Width, float Height) DragSize { get; set; }
	public static (float Width, float Height) FlickSize { get; set; }
	public static (float Width, float Height) HoldHeadSize { get; set; }
	public static (float Width, float Height) HoldBodySize { get; set; }
	public static (float Width, float Height) HoldEndSize { get; set; }
	public static (float Width, float Height) MultiTapSize { get; set; }
	public static (float Width, float Height) MultiDragSize { get; set; }
	public static (float Width, float Height) MultiFlickSize { get; set; }
	public static (float Width, float Height) MultiHoldHeadSize { get; set; }
	public static (float Width, float Height) MultiHoldBodySize { get; set; }
	public static (float Width, float Height) MultiHoldEndSize { get; set; }

	public static (float Width, float Height) HitFXSize { get; set; }

	public static (float Width, float Height) JudgeLineSize { get; set; }

	public static Dictionary<string, (float Width, float Height)> CustomTextureSizes { get; set; } = new();

	private static readonly Dictionary<string, string> TextureUrls = new()
	{
		{ "Tap", @"/Assets/WebPhiCharter/Tap.png" },
		{ "Drag", @"/Assets/WebPhiCharter/Drag.png" },
		{ "Flick", @"/Assets/WebPhiCharter/Flick.png" },
		{ "HoldHead", @"/Assets/WebPhiCharter/HoldHead.png" },
		{ "HoldBody", @"/Assets/WebPhiCharter/Hold.png" },
		{ "HoldEnd", @"/Assets/WebPhiCharter/HoldEnd.png" },
		{ "MultiTap", @"/Assets/WebPhiCharter/TapHL.png" },
		{ "MultiDrag", @"/Assets/WebPhiCharter/DragHL.png" },
		{ "MultiFlick", @"/Assets/WebPhiCharter/FlickHL.png" },
		{ "MultiHoldHead", @"/Assets/WebPhiCharter/HoldHeadHL.png" },
		{ "MultiHoldBody", @"/Assets/WebPhiCharter/HoldHL.png" },
		{ "MultiHoldEnd", @"/Assets/WebPhiCharter/HoldEndHL.png" },
		{ "HitFX", @"/Assets/WebPhiCharter/HitFXRaw.png" },
		{ "JudgeLine", @"/Assets/WebPhiCharter/JudgeLine.png" }
	};
	public static readonly Dictionary<(bool multi, InternalNoteType type), string> TexturesToNames = new()
	{
		{ (false, InternalNoteType.Tap), "Tap" },
		{ (false, InternalNoteType.Drag), "Drag" },
		{ (false, InternalNoteType.Flick), "Flick" },
		{ (true, InternalNoteType.Tap), "MultiTap" },
		{ (true, InternalNoteType.Drag), "MultiDrag" },
		{ (true, InternalNoteType.Flick), "MultiFlick" }
	};
	public static void Initialize()
	{
		foreach (var info in TextureUrls)
		{
			Misc.CanvasHelperHolder.AddImageByUrl(info.Value, info.Key);
		}
	}
	[JSInvokable]
	public static void JSCallBack(string name, float width, float height)
	{
		PropertyInfo pInfo = typeof(TextureManager).GetProperty(name + "Size")!;
		pInfo.SetValue(null, (width, height));

	}
	public static async void AddImage(string url, string name)
	{
		Misc.CanvasHelperHolder.AddImageByUrl(url, name);
		CustomTextureSizes.Add(name, await Misc.CanvasHelperHolder.GetImageSize(name));
	}
	public static (string name, float width, float height) GetTextureNonHold(InternalNoteType type, bool multi)
	{
		switch (multi, type)
		{
			case (false, InternalNoteType.Tap):
				return ("Tap", TapSize.Width, TapSize.Height);
			case (false, InternalNoteType.Drag):
				return ("Drag", DragSize.Width, DragSize.Height);
			case (false, InternalNoteType.Flick):
				return ("Flick", FlickSize.Width, FlickSize.Height);
			case (true, InternalNoteType.Tap):
				return ("MultiTap", MultiTapSize.Width, MultiTapSize.Height);
			case (true, InternalNoteType.Drag):
				return ("MultiDrag", MultiDragSize.Width, MultiDragSize.Height);
			case (true, InternalNoteType.Flick):
				return ("MultiFlick", MultiFlickSize.Width, MultiFlickSize.Height);
			default: throw new Exception("Unexpected type");
		}
	}
}
