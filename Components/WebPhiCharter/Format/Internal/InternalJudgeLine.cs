using System.Numerics;

namespace yt6983138.github.io.Components.WebPhiCharter;
public class InternalJudgeLine
{
	private int _moveEventIndex = 0;
	private int _rotateEventIndex = 0;
	private int _opacityEventIndex = 0;

	private int _bpmEventIndex = 0;

	private int _speedEventIndex = 0;
	private int _lastSpeedEventUpdateTimeMS = 0;
	private float _elpasedNotePos = 0;

	// please use public Events property if the event WILL BE CHANGED
	private List<InternalMoveEvent> _moveEvents = new();
	private List<InternalRotationEvent> _rotateEvents = new();
	private List<InternalOpacityEvent> _opacityEvents = new();
	private List<InternalSpeedEvent> _speedEvents = new();
	private List<InternalBPMEvent> _bpmEvents = new();

	public List<InternalNote> Notes { get; set; } = new();
	public Dictionary<int, InternalNoteAdditionalEvent> AdditionalEventForNote { get; set; } = new();
	public Dictionary<int, int> NoteMultiHoldMS { get; set; } = new();

	private Vector2 _currentPos = Vector2.Zero;
	private float _currentRotationEuler = 0;
	public Vector2 CurrentPos { get { return _currentPos; } }
	public float CurrentRotationEuler { get { return _currentRotationEuler; } }


	public InternalJudgeLine(float baseBpm)
	{
		_bpmEvents.Add(new InternalBPMEvent() { Bpm = baseBpm, StartTimeRelativeToLast = new BeatInfo() { Beat = 0, DivisionLevel = 0, DivisionNumerator = 0 } });
	}
	public T GetValueFromEventList<T>(int index) where T : notnull, InternalEvent
	{
		switch (Activator.CreateInstance(typeof(T))) // pattern matching sux
		{
			case InternalMoveEvent _:
				return (T)_moveEvents[index].QuickCopy();
			case InternalRotationEvent _:
				return (T)_rotateEvents[index].QuickCopy();
			case InternalOpacityEvent _:
				return (T)_opacityEvents[index].QuickCopy();
			case InternalSpeedEvent _:
				return (T)_speedEvents[index].QuickCopy();
			default:
				throw new Exception("Unregistered event!");
		}
	}
	public int GetCountOfEventList<T>() where T : notnull, InternalEvent
	{
		switch (Activator.CreateInstance(typeof(T))) // pattern matching sux
		{
			case InternalMoveEvent _:
				return _moveEvents.Count;
			case InternalRotationEvent _:
				return _rotateEvents.Count;
			case InternalOpacityEvent _:
				return _opacityEvents.Count;
			case InternalSpeedEvent _:
				return _speedEvents.Count;
			default:
				throw new Exception("Unregistered event!");
		}
	}
	public void SetValueToEventList<T>(int index, T obj) where T : notnull, InternalEvent
	{
		switch (obj) // pattern matching sux
		{
			case InternalMoveEvent @event:
				_moveEvents[index] = @event;
				break;
			case InternalRotationEvent @event:
				_rotateEvents[index] = @event;
				break;
			case InternalOpacityEvent @event:
				_opacityEvents[index] = @event;
				break;
			case InternalSpeedEvent @event:
				_elpasedNotePos += @event.GetIntegral() - _speedEvents[index].GetIntegral();
				_speedEvents[index] = @event;
				break;
			default:
				throw new Exception("Unregistered event!");
		}
	}
	public void InsertValueToEventList<T>(int index, T obj) where T : notnull, InternalEvent
	{
		switch (obj)
		{
			case InternalMoveEvent @event:
				_moveEvents.Insert(index, @event);
				break;
			case InternalRotationEvent @event:
				_rotateEvents.Insert(index, @event);
				break;
			case InternalOpacityEvent @event:
				_opacityEvents.Insert(index, @event);
				break;
			case InternalSpeedEvent @event:
				if (index <= _speedEventIndex)
				{
					_elpasedNotePos += @event.GetIntegral();
					_speedEventIndex++;
				}
				_speedEvents.Insert(index, @event);
				break;
			default:
				throw new Exception("Unregistered event!");
		}
	}
	public void AddValueToEventList<T>(T obj) where T : notnull, InternalEvent
	{
		switch (obj)
		{
			case InternalMoveEvent @event:
				_moveEvents.Add(@event);
				break;
			case InternalRotationEvent @event:
				_rotateEvents.Add(@event);
				break;
			case InternalOpacityEvent @event:
				_opacityEvents.Add(@event);
				break;
			case InternalSpeedEvent @event:
				_speedEvents.Add(@event);
				break;
			default:
				throw new Exception("Unregistered event!");
		}
	}
	public void RemoveValueFromEventList<T>(int index) where T : notnull, InternalEvent
	{
		switch (Activator.CreateInstance(typeof(T))) // pattern matching sux
		{
			case InternalMoveEvent _: // those dont have side effect
				if (index <= _moveEventIndex) _moveEventIndex--;
				_moveEvents.RemoveAt(index);
				break;
			case InternalRotationEvent _:
				if (index <= _rotateEventIndex) _rotateEventIndex--;
				_rotateEvents.RemoveAt(index);
				break;
			case InternalOpacityEvent _:
				if (index <= _opacityEventIndex) _opacityEventIndex--;
				_opacityEvents.RemoveAt(index);
				break;
			case InternalSpeedEvent _:
				if (index < _speedEventIndex)
				{
					InternalSpeedEvent @event = _speedEvents[index];
					_elpasedNotePos -= @event.GetIntegral();
					_speedEventIndex--;
				}
				if (index == _speedEventIndex)
				{
					InternalSpeedEvent @event = _speedEvents[index];
					int startTimeMS = @event.StartTime.GetMS(in _bpmEvents);
					int endTimeMS = @event.EndTime.GetMS(in _bpmEvents);
					_elpasedNotePos -= @event.GetIntegral(rangeMax: (_lastSpeedEventUpdateTimeMS - startTimeMS) / (endTimeMS - startTimeMS));
					_speedEventIndex--;
				}
				_speedEvents.RemoveAt(index);
				break;
			default:
				throw new Exception("Unregistered event!");
		}
	}
}
