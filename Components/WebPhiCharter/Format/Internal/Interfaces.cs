namespace yt6983138.github.io.Components.WebPhiCharter;

public interface IInternalEvents
{
	public BeatInfo StartTime { get; set; }
	public BeatInfo EndTime { get; set; }
	public InternalEventInterpolateMode InterpolateMode { get; set; }
	/// <summary>
	/// custom function to control pos
	/// todo: add further explaination 
	/// </summary>
	public string? CustomInterpolateFunction { get; set; }
}
