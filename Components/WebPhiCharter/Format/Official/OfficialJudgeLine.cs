using System.Diagnostics.CodeAnalysis;

namespace yt6983138.github.io.Components.WebPhiCharter;

#pragma warning disable CS8618
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class OfficialJudgeLine
{
	public float bpm { get; set; }
	public List<OfficialNote> notesAbove { get; set; }
	public List<OfficialNote> notesBelow { get; set; }
	public List<OfficialSpeedEvent> speedEvents { get; set; }
	public List<OfficialMoveEvent> judgeLineMoveEvents { get; set; }
	public List<OfficialRotateEvent> judgeLineRotateEvents { get; set; }
	public List<OfficialDisappearEvent> judgeLineDisappearEvents { get; set; } // to prevent u being confused on this one - disappear = opacity
}
#pragma warning restore
