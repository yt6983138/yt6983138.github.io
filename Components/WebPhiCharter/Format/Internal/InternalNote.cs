using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using System.Numerics;

namespace yt6983138.github.io.Components.WebPhiCharter;
public class InternalNote
{
	public required InternalNoteType NoteType { get; set; }
	/// <summary>
	/// PosX 1 = 1 Screen width (canvas width)
	/// </summary>
	public required float PosX { get; set; }
	/// <summary>
	/// PosY 1 = 1 Screen height (canvas height)
	/// </summary>
	public float PosY { get; private set; }
	public BeatInfo Time { get; private set; }
	public int TimeMS { get; private set; }

	/// <summary>
	/// Unused when NoteType != hold, this will iterate thru bpm&flow speed list to find length <br></br>
	/// the time length is relative to Time, ex. hold length 1 0/0 will make the hold has 1 beat length.
	/// </summary>
	public BeatInfo HoldLength { get; private set; } = BeatInfo.Zero;
	public float HoldRenderLength { get; private set; } = 0;

	public int? ID { get; set; } = null;

	private Vector2 PosNow = Vector2.Zero;
	private float RotationNow = 0;

	public InternalNote(in List<InternalBPMEvent> bpmEvents, in List<InternalSpeedEvent> speedEvents, BeatInfo time, BeatInfo? holdLength = null)
	{
		SetTime(bpmEvents, speedEvents, time);
		if (holdLength != null)
		{
			SetHoldLength(bpmEvents, speedEvents, (BeatInfo)holdLength);
		}
	}
	// no auto properties sad
	public void SetHoldLength(in List<InternalBPMEvent> bpmEvents, in List<InternalSpeedEvent> speedEvents, BeatInfo length)
	{
		throw new NotImplementedException();
	}
	public void SetTime(in List<InternalBPMEvent> bpmEvents, in List<InternalSpeedEvent> speedEvents, BeatInfo Time)
	{
		throw new NotImplementedException();
	}
	public void Update(int timeMS, float elpasedNotePos, in Dictionary<int, InternalNoteAdditionalEvent> additionalEvents, in InternalJudgeLine parent, ref BECanvasComponent component, ref Canvas2DContext context)
	{
		if (!Verify()) throw new Exception("Invaild properties found!");
		this.PosNow.X = this.PosX;
		this.PosNow.Y = this.PosY - elpasedNotePos;

		InternalNoteAdditionalEvent @event = default!;
		if (this.ID != null)
		{
			additionalEvents.TryGetValue((int)this.ID, out @event!);
		}
		Draw(ref component, ref context, parent, timeMS, @event!);

	}
	public void Draw(ref BECanvasComponent component, ref Canvas2DContext context, in InternalJudgeLine parent, int timeMS, InternalNoteAdditionalEvent additionalEvent = default!)
	{

	}
	/// <summary>
	/// check if anything goes wrong
	/// </summary>
	/// <returns>
	/// false if something went wrong
	/// </returns>
	public bool Verify()
	{
		if (NoteType == InternalNoteType.Custom) return false;
		return true;
	}

}
