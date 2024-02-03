using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System.Numerics;

namespace yt6983138.github.io.Components.WebPhiCharter;
public class InternalNote
{
	public required InternalNoteType NoteType { get; set; }
	/// <summary>
	/// PosX 1 = 1 Screen width (canvas width)
	/// </summary>
	public required float PosX { get; set; }
	public required bool IsDownSide { get; set; }
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

	public InternalNote(in List<InternalBPMEvent> bpmEvents, in List<InternalSpeedEvent> speedEvents, BeatInfo time, BeatInfo? holdLength = null)
	{
		this.SetTime(in bpmEvents, in speedEvents, time);
		if (holdLength != null)
		{
			this.SetHoldLength(in bpmEvents, in speedEvents, holdLength);
		}
	}
	public InternalNote(in List<InternalBPMEvent> bpmEvents, in List<InternalSpeedEvent> speedEvents, OfficialNote officialNote)
	{
		InternalNoteType type = (InternalNoteType)officialNote.type;
		this.SetTime(in bpmEvents, in speedEvents, Utils.OfficialChartTimeToBeatInfo(officialNote.time));
		if (officialNote.holdTime != 0 && type == InternalNoteType.Hold)
		{
			this.SetHoldLength(in bpmEvents, in speedEvents, Utils.OfficialChartTimeToBeatInfo(officialNote.holdTime));
		}
	}
	// no auto properties sad
	public void SetHoldLength(in List<InternalBPMEvent> bpmEvents, in List<InternalSpeedEvent> speedEvents, BeatInfo length)
	{
		int lengthTimeMS = (this.Time + length).GetMS(in bpmEvents) - this.TimeMS;
		float integral = 0;
		for (int i = 0; i < speedEvents.Count; i++)
		{
			var @event = speedEvents[i];
			if (this.TimeMS + lengthTimeMS < @event.StartMS)
			{
				break; // |note| ... | event | timeline
			}
			if (@event.StartMS < this.TimeMS && this.TimeMS < @event.EndMS) // |      event      |
			{                                                               //   | note ... | (length unknown)
				float start = 1;
				if (lengthTimeMS > @event.EndMS) // |      event      |
				{                                //   | note ...         | (length exceeds event end time)
					start = (float)(lengthTimeMS - @event.StartMS) / (@event.EndMS - @event.StartMS);
				}
				integral += @event.GetIntegral(rangeMin: start);
			}
			if (this.TimeMS < @event.StartMS && @event.EndMS < lengthTimeMS) //       | event |
			{                                                                // | note ...        | (entirely larger than event)
				integral += @event.GetIntegral();
			}
			if (this.TimeMS < @event.StartMS && @event.StartMS < lengthTimeMS && lengthTimeMS < @event.EndMS)
			{
				integral += @event.GetIntegral(0, (float)(lengthTimeMS - @event.StartMS) / (@event.EndMS - @event.StartMS));
			}
		}
		this.HoldRenderLength = integral;
		this.HoldLength = length;
	}
	public void SetTime(in List<InternalBPMEvent> bpmEvents, in List<InternalSpeedEvent> speedEvents, BeatInfo Time)
	{
		int timeMS = Time.GetMS(in bpmEvents);
		float integral = 0;
		for (int i = 0; i < speedEvents.Count; i++)
		{
			var @event = speedEvents[i];
			if (@event.EndMS < timeMS)
			{
				integral += @event.GetIntegral();
			}
			if (@event.StartMS < timeMS && timeMS < @event.EndMS)
			{
				integral += @event.GetIntegral(0, (float)(timeMS - @event.StartMS) / (@event.EndMS - @event.StartMS));
			}
			if (timeMS < @event.StartMS)
			{
				break;
			}
		}
		this.PosY = integral;
		this.Time = Time;
		this.TimeMS = timeMS;

		if (this.NoteType == InternalNoteType.Hold && this.HoldRenderLength > 0) this.SetHoldLength(in bpmEvents, in speedEvents, this.HoldLength);
	}
	public object QuickCopy() => this.MemberwiseClone();
}
