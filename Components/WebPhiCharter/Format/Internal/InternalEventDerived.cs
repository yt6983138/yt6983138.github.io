using System.Numerics;

namespace yt6983138.github.io.Components.WebPhiCharter;
public class InternalMoveEvent : InternalEvent
{
	/// <summary>
	/// x&y: 1 unit = 1 * screen(canvas) width/height
	/// </summary>
	public required Vector2 MovementStart { get; set; }
	/// <summary>
	/// <inheritdoc cref="MovementStart" path="/summary"/>
	/// </summary>
	public required Vector2 MovementEnd { get; set; }

#pragma warning disable CS0809
	[Obsolete("Use InterpolateX/Y instead!")] // idk better attr
	public override InternalEventInterpolateMode InterpolateMode { get => base.InterpolateMode; set => base.InterpolateMode = value; }
	[Obsolete("Use InterpolateX/Y instead!")]
	public override EaseMode EaseMode { get => base.EaseMode; set => base.EaseMode = value; }
	[Obsolete("Use InterpolateX/Y instead!")]
	public override string? CustomInterpolateFunction { get => base.CustomInterpolateFunction; set => base.CustomInterpolateFunction = value; }
#pragma warning restore CS0809

	public InternalMoveEvent(in List<InternalBPMEvent> bpmEvents, BeatInfo startTime, BeatInfo endTime)
	{
		this.SetStartTime(in bpmEvents, startTime);
		this.SetEndTime(in bpmEvents, endTime);
	}
	private InternalMoveEvent() { }
	public InternalEventInterpolateMode InterpolateModeX { get; set; } = InternalEventInterpolateMode.Linear;
	public EaseMode EaseModeX { get; set; } = EaseMode.EaseBoth;
	public string? CustomInterpolateFunctionX { get; set; } = null;
	public InternalEventInterpolateMode InterpolateModeY { get; set; } = InternalEventInterpolateMode.Linear;
	public EaseMode EaseModeY { get; set; } = EaseMode.EaseBoth;
	public string? CustomInterpolateFunctionY { get; set; } = null;
}
public class InternalRotationEvent : InternalEvent
{
	/// <summary>
	/// euler angles
	/// </summary>
	public required float RotationStart { get; set; }
	/// <summary>
	/// <inheritdoc cref="RotationStart" path="/summary"/>
	/// </summary>
	public required float RotationEnd { get; set; }

	public InternalRotationEvent(in List<InternalBPMEvent> bpmEvents, BeatInfo startTime, BeatInfo endTime)
	{
		this.SetStartTime(in bpmEvents, startTime);
		this.SetEndTime(in bpmEvents, endTime);
	}
	private InternalRotationEvent() { }
}
public class InternalOpacityEvent : InternalEvent
{
	/// <summary>
	/// 0 ~ 1, 0: fully transparent, 1: fully opaque 
	/// </summary>
	public required float OpacityStart { get; set; }
	/// <summary>
	/// <inheritdoc cref="OpacityStart" path="/summary"/>
	/// </summary>
	public required float OpacityEnd { get; set; }

	public InternalOpacityEvent(in List<InternalBPMEvent> bpmEvents, BeatInfo startTime, BeatInfo endTime)
	{
		this.SetStartTime(in bpmEvents, startTime);
		this.SetEndTime(in bpmEvents, endTime);
	}
	private InternalOpacityEvent() { }
}
public class InternalSpeedEvent : InternalEvent
{
	/// <summary>
	/// 1 unit = 1 screen(canvas) height 
	/// </summary>
	public required float SpeedStart { get; set; }
	/// <summary>
	/// <inheritdoc cref="OpacityStart" path="/summary"/>
	/// </summary>
	public required float SpeedEnd { get; set; }
	public override InternalEventInterpolateMode InterpolateMode { get { return InternalEventInterpolateMode.Linear; } set { return; } }
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public override string? CustomInterpolateFunction { get { return "Unsupported Operation."; } set { return; } }

	public InternalSpeedEvent(in List<InternalBPMEvent> bpmEvents, BeatInfo startTime, BeatInfo endTime)
	{
		this.SetStartTime(in bpmEvents, startTime);
		this.SetEndTime(in bpmEvents, endTime);
	}
	private InternalSpeedEvent() { }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="rangeMin">0 ~ 1</param>
	/// <param name="rangeMax">0 ~ 1</param>
	/// <returns></returns>
	public float GetIntegral(float rangeMin = 0, float rangeMax = 1)
	{
		float delta = this.SpeedEnd - this.SpeedStart;
		switch (this.InterpolateMode)
		{
			case InternalEventInterpolateMode.Linear:
				return ((this.SpeedStart + delta * rangeMin) + (this.SpeedStart + delta * rangeMax)) * 2;
			case InternalEventInterpolateMode.Log10:
				throw new NotImplementedException();
			case InternalEventInterpolateMode.Square:
				throw new NotImplementedException();
			case InternalEventInterpolateMode.Expo2:
				throw new NotImplementedException();
			case InternalEventInterpolateMode.Sine:
				throw new NotImplementedException();
			case InternalEventInterpolateMode.Cutoff:
				throw new NotImplementedException();
			case InternalEventInterpolateMode.Custom:
			default:
				throw new NotSupportedException("Custom interpolate mode not supported.");
		}

	}
}
public class InternalBPMEvent : IInternalSpecialEvents
{
	public required float Bpm { get; set; }
	/// <summary>
	/// using last bpm from last event
	/// </summary>
	public required BeatInfo StartTimeRelativeToLast { get; set; }

	public object QuickCopy()
	{
		return this.MemberwiseClone();
	}
}
public class InternalLineAdditionalEvent
{
	// placeholder
}
public class InternalNoteAdditionalEvent
{
	public bool IsFake { get; set; } = false;
	public float Speed { get; set; } = 1;
	public float Opacity { get; set; } = 1;
	public int VisibleSinceMS { get; set; } = 0;
	public int VisibleTime { get; set; } = int.MaxValue;
	public (bool? multitap, InternalNoteType? type) Texture { get; set; } = (null, null);
	public string? CustomTexturePath { get; set; } = null;
	public Vector2 Scale { get; set; } = new Vector2(1, 1);
	public Matrix3x2 RenderTransform { get; set; } = new Matrix3x2(1, 0, 0, 1, 0, 0);
}