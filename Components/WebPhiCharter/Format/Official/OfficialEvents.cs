using System.Diagnostics.CodeAnalysis;

namespace yt6983138.github.io.Components.WebPhiCharter;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class OfficialSpeedEvent
{
	public float startTime { get; set; }
	public float endTime { get; set; }
	public float value { get; set; }
}
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class OfficialMoveEvent
{
	public float startTime { get; set; }
	public float endTime { get; set; }
	public float start { get; set; }
	public float end { get; set; }
	public float start2 { get; set; }
	public float end2 { get; set; }
}
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class OfficialRotateEvent
{
	public float startTime { get; set; }
	public float endTime { get; set; }
	public float start { get; set; }
	public float end { get; set; }
}
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class OfficialDisappearEvent
{
	public float startTime { get; set; }
	public float endTime { get; set; }
	public float start { get; set; }
	public float end { get; set; }
}
