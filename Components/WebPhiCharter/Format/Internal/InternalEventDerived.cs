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

	/// <summary>
	/// 
	/// </summary>
	/// <param name="rangeMin">0 ~ 1</param>
	/// <param name="rangeMax">0 ~ 1</param>
	/// <returns></returns>
	public float GetIntegral(float rangeMin = 0, float rangeMax = 1)
	{
		float delta = SpeedEnd - SpeedStart;
		return ((SpeedStart + delta * rangeMin) + (SpeedStart + delta * rangeMax)) * 2;
	}
}
public class InternalBPMEvent : IInternalSpecialEvents
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