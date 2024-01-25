using System.Diagnostics.CodeAnalysis;

namespace yt6983138.github.io.Components.WebPhiCharter;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class OfficialNote
{
	/// <summary>
	/// 1: tap
	/// 2: drag
	/// 3: hold
	/// 4: flick
	/// </summary>
	public byte type { get; set; } // do not fix name violation on those
	/// <summary>
	/// time 32 bpm 60 = 1 sec
	/// time 64 bpm 120 = 1 sec
	/// </summary>
	public float time { get; set; }
	/// <summary>
	/// uh same as time ig
	/// </summary>
	public float holdTime { get; set; }
	/// <summary>
	/// speed, 1 = 1x
	/// </summary>
	public float speed { get; set; }
	/// <summary>
	/// -Resource.OfficalChartPosXMagicNumber ~ +Resource.OfficalChartPosXMagicNumber 
	/// </summary>
	public float positionX { get; set; }
	/// <summary>
	/// floor pos 1 = 0.5 screen height
	/// </summary>
	public float floorPosition { get; set; }
}
