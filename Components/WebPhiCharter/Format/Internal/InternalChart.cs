namespace yt6983138.github.io.Components.WebPhiCharter;

public class InternalChart
{
	public List<InternalJudgeLine> JudgeLines { get; set; } = new();
	public List<InternalLineAdditionalEvent> LineAdditionalEvents { get; set; } = new();
	public void Update(int timeMs)
	{
		foreach (var line in this.JudgeLines)
		{
			line.Update(timeMs);
		}
	}
}
