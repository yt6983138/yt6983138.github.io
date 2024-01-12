﻿using Microsoft.Extensions.Logging;

namespace yt6983138.github.io.Components.WebPhiCharter;

public partial class InternalJudgeLine
{
	#region Normal event operations
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
				for (int i = 0; i < _moveEvents.Count; i++)
				{
					if (index == i) continue;
					var e = _moveEvents[i];
					if (@event.EndTime < e.StartTime) break;

					@event.CheckOverlap(e);
				}
				_moveEvents[index] = @event;
				break;
			case InternalRotationEvent @event:
				for (int i = 0; i < _rotateEvents.Count; i++)
				{
					if (index == i) continue;
					var e = _rotateEvents[i];
					if (@event.EndTime < e.StartTime) break;

					@event.CheckOverlap(e);
				}
				_rotateEvents[index] = @event;
				break;
			case InternalOpacityEvent @event:
				for (int i = 0; i < _opacityEvents.Count; i++)
				{
					if (index == i) continue;
					var e = _opacityEvents[i];
					if (@event.EndTime < e.StartTime) break;

					@event.CheckOverlap(e);
				}
				_opacityEvents[index] = @event;
				break;
			case InternalSpeedEvent @event:
				for (int i = 0; i < _speedEvents.Count; i++)
				{
					if (index == i) continue;
					var e = _speedEvents[i];
					if (@event.EndTime < e.StartTime) break;

					@event.CheckOverlap(e);
				}
				_elapsedNotePos += @event.GetIntegral() - _speedEvents[index].GetIntegral();
				_speedEvents[index] = @event;
				break;
			default:
				throw new Exception("Unregistered event!");
		}
	}
	[NotRecommended("Use AddValueToEventList instead, it will determine where to add automaticly.")]
	public void InsertValueToEventList<T>(int index, T obj) where T : notnull, InternalEvent
	{
		switch (obj)
		{
			case InternalMoveEvent @event:
				CheckOverlap(@event);
				_moveEvents.Insert(index, @event);
				break;
			case InternalRotationEvent @event:
				CheckOverlap(@event);
				_rotateEvents.Insert(index, @event);
				break;
			case InternalOpacityEvent @event:
				CheckOverlap(@event);
				_opacityEvents.Insert(index, @event);
				break;
			case InternalSpeedEvent @event:
				CheckOverlap(@event);
				if (index <= _speedEventIndex) // no need to worry about changing the event it is parsing now - would be prevented by CheckOverlap
				{
					_elapsedNotePos += @event.GetIntegral();
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
		bool cancel = false;
		bool stopCheckOverlap = false;
		int i = 0;
		switch (obj)
		{
			case InternalMoveEvent @event:
				foreach (var e in _moveEvents)
				{
					if (!stopCheckOverlap) @event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) stopCheckOverlap = true;
					if (@event.StartTime > e.StartTime && !cancel) { _moveEvents.Insert(i, @event); cancel = true; }
					else i++;
					if (cancel && stopCheckOverlap) break;
				}
				break;
			case InternalRotationEvent @event:
				foreach (var e in _rotateEvents)
				{
					if (!stopCheckOverlap) @event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) stopCheckOverlap = true;
					if (@event.StartTime > e.StartTime && !cancel) { _rotateEvents.Insert(i, @event); cancel = true; }
					else i++;
					if (cancel && stopCheckOverlap) break;
				}
				break;
			case InternalOpacityEvent @event:
				foreach (var e in _opacityEvents)
				{
					if (!stopCheckOverlap) @event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) stopCheckOverlap = true;
					if (@event.StartTime > e.StartTime && !cancel) { _opacityEvents.Insert(i, @event); cancel = true; }
					else i++;
					if (cancel && stopCheckOverlap) break;
				}
				break;
			case InternalSpeedEvent @event:
				foreach (var e in _speedEvents)
				{
					if (!stopCheckOverlap) @event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) stopCheckOverlap = true;
					if (@event.StartTime > e.StartTime && !cancel)
					{
						if (i <= _speedEventIndex)
						{
							_elapsedNotePos += @event.GetIntegral();
							_speedEventIndex++;
						}
						_speedEvents.Insert(i, @event);
						cancel = true;
					}
					else i++;
					if (cancel && stopCheckOverlap) break;
				}
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
					_elapsedNotePos -= @event.GetIntegral();
					_speedEventIndex--;
				}
				if (index == _speedEventIndex)
				{
					InternalSpeedEvent @event = _speedEvents[index];
					_elapsedNotePos -= @event.GetIntegral(rangeMax: (_lastSpeedEventUpdateTimeMS - @event.StartMS) / (@event.EndMS - @event.StartMS));
					_speedEventIndex--;
				}
				_speedEvents.RemoveAt(index);
				break;
			default:
				throw new Exception("Unregistered event!");
		}
	}
	public void CheckOverlap<T>(T obj) where T : InternalEvent
	{
		switch (obj)
		{
			case InternalMoveEvent @event:
				foreach (var e in _moveEvents)
				{
					@event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) break;
				}
				break;
			case InternalRotationEvent @event:
				foreach (var e in _rotateEvents)
				{
					@event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) break;
				}
				break;
			case InternalOpacityEvent @event:
				foreach (var e in _opacityEvents)
				{
					@event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) break;
				}
				break;
			case InternalSpeedEvent @event:
				foreach (var e in _speedEvents)
				{
					@event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) break;
				}
				break;
			default:
				throw new Exception("Unregistered event!");
		}
	}
	#endregion

	#region Special event operations
	private void ModifyEventTimeByBpmList()
	{
		throw new NotImplementedException();
	}
	public InternalBPMEvent GetBpmEventValue(int index)
	{
		return (InternalBPMEvent)_bpmEvents[index].QuickCopy();
	}
	public void AddBpmEvent(InternalBPMEvent @event, bool rebuildEverything = false)
	{
		_bpmEvents.Add(@event);
		if (rebuildEverything)
		{
			ModifyEventTimeByBpmList();
		}
	}
	public void RemoveBpmEvent(int index, bool maintainStructure = true, bool rebuildEverything = false)
	{
		if (maintainStructure && index != _bpmEvents.Count && false) // kinda lazy to impl it now
		{
			var eventNow = _bpmEvents[index];
			var eventNext = _bpmEvents[index + 1];

			_bpmEvents[index + 1].StartTimeRelativeToLast = eventNext.StartTimeRelativeToLast + eventNow.StartTimeRelativeToLast * new BeatInfo() { Beat = 0, DivisionLevel = (int)eventNext.Bpm, DivisionNumerator = (int)eventNow.Bpm };
		}
		_bpmEvents.RemoveAt(index);
		if (rebuildEverything) ModifyEventTimeByBpmList();
	}
	public void InsertBpmEvent(int index, InternalBPMEvent @event, bool rebuildEverything = false)
	{
		_bpmEvents.Insert(index, @event);
		if (rebuildEverything) ModifyEventTimeByBpmList();
	}
	public void ModifyBpmEvent(int index, InternalBPMEvent @event, bool rebuildEverything = false)
	{
		_bpmEvents[index] = @event;
		if (rebuildEverything) ModifyEventTimeByBpmList();
	}
	#endregion
}