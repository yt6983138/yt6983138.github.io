namespace yt6983138.github.io.Components.WebPhiCharter;
public abstract class InternalEvent
{
	public required virtual BeatInfo StartTime { get; set; }
	public required virtual BeatInfo EndTime { get; set; }
	public virtual InternalEventInterpolateMode InterpolateMode { get; set; }
	/// <summary>
	/// custom function to control pos
	/// todo: add further explaination 
	/// </summary>
	public virtual string? CustomInterpolateFunction { get; set; }
	public virtual object QuickCopy()
	{
		return this.MemberwiseClone();
	}
}