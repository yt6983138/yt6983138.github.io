using System.Numerics;

namespace yt6983138.github.io.Components.WebPhiCharter;
public class InternalMoveEvent : IInternalEvents
{
	/// <summary>
	/// x&y: 1 unit = 1 * screen(canvas) width/height
	/// </summary>
	public required Vector2 MovementStart { get; set; }
	/// <summary>
	/// <inheritdoc cref="MovementStart" path="/summary"/>
	/// </summary>
	public required Vector2 MovementEnd { get; set; }
	public required BeatInfo StartTime { get; set; }
	public required BeatInfo EndTime { get; set; }
	public InternalEventInterpolateMode InterpolateMode { get; set; } = InternalEventInterpolateMode.Linear;
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public string? CustomInterpolateFunction { get; set; } = null;
}
public class InternalRotationEvent : IInternalEvents
{
	/// <summary>
	/// euler angles
	/// </summary>
	public required float RotationStart { get; set; }
	/// <summary>
	/// <inheritdoc cref="RotationStart" path="/summary"/>
	/// </summary>
	public required float RotationEnd { get; set; }
	public required BeatInfo StartTime { get; set; }
	public required BeatInfo EndTime { get; set; }
	public InternalEventInterpolateMode InterpolateMode { get; set; } = InternalEventInterpolateMode.Linear;
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public string? CustomInterpolateFunction { get; set; } = null;
}
public class InternalOpacityEvent : IInternalEvents
{
	/// <summary>
	/// 0 ~ 1, 0: fully transparent, 1: fully opaque 
	/// </summary>
	public required float OpacityStart { get; set; }
	/// <summary>
	/// <inheritdoc cref="OpacityStart" path="/summary"/>
	/// </summary>
	public required float OpacityEnd { get; set; }
	public required BeatInfo StartTime { get; set; }
	public required BeatInfo EndTime { get; set; }
	public InternalEventInterpolateMode InterpolateMode { get; set; } = InternalEventInterpolateMode.Linear;
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public string? CustomInterpolateFunction { get; set; } = null;
}
public class InternalSpeedEvent : IInternalEvents
{
	/// <summary>
	/// 1 unit = 1 screen(canvas) height 
	/// </summary>
	public required float SpeedStart { get; set; }
	/// <summary>
	/// <inheritdoc cref="OpacityStart" path="/summary"/>
	/// </summary>
	public required float SpeedEnd { get; set; }
	public required BeatInfo StartTime { get; set; }
	public required BeatInfo EndTime { get; set; }
	public InternalEventInterpolateMode InterpolateMode { get; set; } = InternalEventInterpolateMode.Linear;
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public string? CustomInterpolateFunction { get; set; } = null;
}
public class InternalBPMEvent
{
	public required float Bpm { get; set; }
	public required BeatInfo StartTimeRelativeToLast { get; set; }
}
public class InternalLineAdditionalEvent
{
	// placeholder
}
public class InternalNoteAdditionalEvent
{
	public bool IsFake { get; set; } = false;
	public float Speed { get; set; } = 1;
	public (bool? multitap, InternalNoteType? type) Texture { get; set; } = (null, null);
	public string? CustomTexturePath { get; set; } = null;
	public Vector2 Scale { get; set; } = Vector2.One;
	public Matrix3x2 RenderTransform { get; set; } = new Matrix3x2(1, 0, 0, 1, 0, 0);
	public Vector2 Anchor { get; set; } = new Vector2(0.5f, 0.5f);
	public Vector2 Pivot { get; set; } = new Vector2(0.5f, 0.5f);
}