using System.Diagnostics.CodeAnalysis;

namespace yt6983138.github.io.Components.WebPhiCharter;

#pragma warning disable CS8618
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class OfficialChart
{
	public int formatVersion { get; set; }
	public float offset { get; set; }
	public List<OfficialJudgeLine> judgeLineList { get; set; }
}
#pragma warning restore
