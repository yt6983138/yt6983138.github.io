using System.Numerics;

namespace yt6983138.github.io.Components.WebPhiCharter;
public class InternalJudgeLine
{
	private int _moveEventIndex = 0;
	private int _rotateEventIndex = 0;
	private int _opacityEventIndex = 0;
	private int _speedEventIndex = 0;
	private int _lastSpeedEventUpdateTimeMS = 0;
	private int _bpmEventIndex = 0;

	// please use public Events property if the event WILL BE CHANGED
	private List<InternalMoveEvent> _moveEvents = new();
	private List<InternalRotationEvent> _rotateEvents = new();
	private List<InternalOpacityEvent> _opacityEvents = new();
	private List<InternalSpeedEvent> _speedEvents = new();
	private List<InternalBPMEvent> _bpmEvents = new();

	// please use private fields if the event WILL NOT BE CHANGED
	public List<InternalMoveEvent> MoveEvents { get { _moveEventIndex = 0; return _moveEvents; } set { _moveEvents = value; _moveEventIndex = 0; } }
	public List<InternalRotationEvent> RotationEvents { get { _rotateEventIndex = 0; return _rotateEvents; } set { _rotateEvents = value; _rotateEventIndex = 0; } }
	public List<InternalOpacityEvent> OpacityEvents { get { _opacityEventIndex = 0; return _opacityEvents; } set { _opacityEvents = value; _opacityEventIndex = 0; } }
	public List<InternalSpeedEvent> SpeedEvents { get { _lastSpeedEventUpdateTimeMS = 0; _speedEventIndex = 0; return _speedEvents; } set { _speedEvents = value; _speedEventIndex = 0; _lastSpeedEventUpdateTimeMS = 0; } }
	public List<InternalBPMEvent> BPMEvents { get { _bpmEventIndex = 0; return _bpmEvents; } set { _bpmEvents = value; _bpmEventIndex = 0; } }

	public List<InternalNote> Notes { get; set; } = new();
	public Dictionary<int, InternalNoteAdditionalEvent> AdditionalEventForNote { get; set; } = new();

	public Vector2 CurrentPos { get; private set; } = Vector2.Zero;
	public float CurrentRotationEuler { get; private set; } = 0;

	public InternalJudgeLine(float baseBpm)
	{
		BPMEvents.Add(new InternalBPMEvent() { Bpm = baseBpm, StartTimeRelativeToLast = new BeatInfo() { Beat = 0, DivisionLevel = 0, DivisionNumerator = 0 } });
	}

}
