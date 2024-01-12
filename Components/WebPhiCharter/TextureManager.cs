using Microsoft.AspNetCore.Components;

namespace yt6983138.github.io.Components.WebPhiCharter;
public static class TextureManager
{
	public static ElementReference Tap { get; set; }
	public static ElementReference Drag { get; set; }
	public static ElementReference Flick { get; set; }
	public static ElementReference HoldHead { get; set; }
	public static ElementReference HoldBody { get; set; }
	public static ElementReference HoldEnd { get; set; }
	public static ElementReference MultiTap { get; set; }
	public static ElementReference MultiDrag { get; set; }
	public static ElementReference MultiFlick { get; set; }
	public static ElementReference MultiHoldHead { get; set; }
	public static ElementReference MultiHoldBody { get; set; }
	public static ElementReference MultiHoldEnd { get; set; }

	public static ElementReference LineAP { get; set; }
	public static ElementReference LineFC { get; set; }
	public static ElementReference LineRegular { get; set; }

	public static ElementReference GetNoteTextureNonHold(InternalNoteType type, bool multi)
	{
		switch ((type, multi))
		{
			case (InternalNoteType.Tap, false):
				return Tap;
			case (InternalNoteType.Drag, false):
				return Drag;
			case (InternalNoteType.Flick, false):
				return Flick;
			case (InternalNoteType.Tap, true):
				return MultiTap;
			case (InternalNoteType.Drag, true):
				return MultiDrag;
			case (InternalNoteType.Flick, true):
				return MultiFlick;
			default:
				throw new Exception("did u try to get texture from hold or custom?");
		}
	}
	public static (ElementReference head, ElementReference body, ElementReference end) GetNoteTextureHold(bool multi)
	{
		if (multi) return (MultiHoldHead, MultiHoldBody, MultiHoldEnd);
		return (HoldHead, HoldBody, HoldEnd);
	}
	public static ElementReference GetLineTexture()
	{
		throw new NotImplementedException();
	}
}
