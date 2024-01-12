namespace yt6983138.github.io.Components.WebPhiCharter;

/// <summary>
/// you MUST create a constuctor with set startTime/endTime method inside
/// </summary>
public abstract class InternalEvent
{
	public virtual BeatInfo StartTime { get; private set; }
	public virtual BeatInfo EndTime { get; private set; }
	public virtual int StartMS { get; private set; }
	public virtual int EndMS { get; private set; }
	public virtual InternalEventInterpolateMode InterpolateMode { get; set; } = InternalEventInterpolateMode.Linear;
	public virtual EaseMode EaseMode { get; set; } = EaseMode.EaseBoth;
	/// <summary>
	/// custom function to control pos
	/// todo: add further explanation 
	/// </summary>
	public virtual string? CustomInterpolateFunction { get; set; } = null;

	public virtual object QuickCopy()
	{
		return this.MemberwiseClone();
	}
	public virtual void SetStartTime(in List<InternalBPMEvent> bpmEvents, BeatInfo time)
	{
		this.StartMS = time.GetMS(bpmEvents);
		this.StartTime = time;
	}
	public virtual void SetEndTime(in List<InternalBPMEvent> bpmEvents, BeatInfo time)
	{
		this.EndMS = time.GetMS(bpmEvents);
		this.EndTime = time;
	}
	public virtual void CheckOverlap(InternalEvent e)
	{
		if (this.StartTime < e.EndTime)
		{
			if (this.EndTime > e.StartTime) throw new Exception("Event overlap!");
		}
		else if (this.EndTime > e.StartTime)
		{
			if (this.StartTime < e.EndTime) throw new Exception("Event overlap!");
		}
	}
}