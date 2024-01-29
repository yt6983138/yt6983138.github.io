using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

namespace yt6983138.github.io.Components.WebPhiCharter;

public partial class InternalJudgeLine
{
	#region Normal event operations
	public T GetValueFromEventList<T>(int index) where T : notnull, InternalEvent
	{
		switch (Activator.CreateInstance(typeof(T), true)) // pattern matching sux
		{
			case InternalMoveEvent _:
				return (T)this._moveEvents[index].QuickCopy();
			case InternalRotationEvent _:
				return (T)this._rotateEvents[index].QuickCopy();
			case InternalOpacityEvent _:
				return (T)this._opacityEvents[index].QuickCopy();
			case InternalSpeedEvent _:
				return (T)this._speedEvents[index].QuickCopy();
			default:
				throw new Exception("Unregistered event!");
		}
	}
	public int GetCountOfEventList<T>() where T : notnull, InternalEvent
	{
		switch (Activator.CreateInstance(typeof(T), true)) // pattern matching sux
		{
			case InternalMoveEvent _:
				return this._moveEvents.Count;
			case InternalRotationEvent _:
				return this._rotateEvents.Count;
			case InternalOpacityEvent _:
				return this._opacityEvents.Count;
			case InternalSpeedEvent _:
				return this._speedEvents.Count;
			default:
				throw new Exception("Unregistered event!");
		}
	}
	public void SetValueToEventList<T>(int index, T obj) where T : notnull, InternalEvent
	{
		switch (obj) // pattern matching sux
		{
			case InternalMoveEvent @event:
				for (int i = 0; i < this._moveEvents.Count; i++)
				{
					if (index == i) continue;
					var e = this._moveEvents[i];
					if (@event.EndTime < e.StartTime) break;

					@event.CheckOverlap(e);
				}
				this._moveEvents[index] = @event;
				break;
			case InternalRotationEvent @event:
				for (int i = 0; i < this._rotateEvents.Count; i++)
				{
					if (index == i) continue;
					var e = this._rotateEvents[i];
					if (@event.EndTime < e.StartTime) break;

					@event.CheckOverlap(e);
				}
				this._rotateEvents[index] = @event;
				break;
			case InternalOpacityEvent @event:
				for (int i = 0; i < this._opacityEvents.Count; i++)
				{
					if (index == i) continue;
					var e = this._opacityEvents[i];
					if (@event.EndTime < e.StartTime) break;

					@event.CheckOverlap(e);
				}
				this._opacityEvents[index] = @event;
				break;
			case InternalSpeedEvent @event:
				for (int i = 0; i < this._speedEvents.Count; i++)
				{
					if (index == i) continue;
					var e = this._speedEvents[i];
					if (@event.EndTime < e.StartTime) break;

					@event.CheckOverlap(e);
				}
				this._elapsedNotePos += @event.GetIntegral() - this._speedEvents[index].GetIntegral();
				this._speedEvents[index] = @event;
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
				this.CheckOverlap(@event);
				this._moveEvents.Insert(index, @event);
				break;
			case InternalRotationEvent @event:
				this.CheckOverlap(@event);
				this._rotateEvents.Insert(index, @event);
				break;
			case InternalOpacityEvent @event:
				this.CheckOverlap(@event);
				this._opacityEvents.Insert(index, @event);
				break;
			case InternalSpeedEvent @event:
				this.CheckOverlap(@event);
				if (index <= this._speedEventIndex) // no need to worry about changing the event it is parsing now - would be prevented by CheckOverlap
				{
					this._elapsedNotePos += @event.GetIntegral();
					this._speedEventIndex++;
				}
				this._speedEvents.Insert(index, @event);
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
				if (this._moveEvents.Count == 0) { this._moveEvents.Add(@event); break; }
				foreach (var e in this._moveEvents)
				{
					if (!stopCheckOverlap) @event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) stopCheckOverlap = true;
					if (@event.StartTime > e.StartTime && !cancel) { this._moveEvents.Insert(i, @event); cancel = true; }
					else i++;
					if (cancel && stopCheckOverlap) break;
				}
				break;
			case InternalRotationEvent @event:
				if (this._rotateEvents.Count == 0) { this._rotateEvents.Add(@event); break; }
				foreach (var e in this._rotateEvents)
				{
					if (!stopCheckOverlap) @event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) stopCheckOverlap = true;
					if (@event.StartTime > e.StartTime && !cancel) { this._rotateEvents.Insert(i, @event); cancel = true; }
					else i++;
					if (cancel && stopCheckOverlap) break;
				}
				break;
			case InternalOpacityEvent @event:
				if (this._opacityEvents.Count == 0) { this._opacityEvents.Add(@event); break; }
				foreach (var e in this._opacityEvents)
				{
					if (!stopCheckOverlap) @event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) stopCheckOverlap = true;
					if (@event.StartTime > e.StartTime && !cancel) { this._opacityEvents.Insert(i, @event); cancel = true; }
					else i++;
					if (cancel && stopCheckOverlap) break;
				}
				break;
			case InternalSpeedEvent @event:
				if (this._speedEvents.Count == 0) { this._speedEvents.Add(@event); break; }
				foreach (var e in this._speedEvents)
				{
					if (!stopCheckOverlap) @event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) stopCheckOverlap = true;
					if (@event.StartTime > e.StartTime && !cancel)
					{
						if (i <= this._speedEventIndex)
						{
							this._elapsedNotePos += @event.GetIntegral();
							this._speedEventIndex++;
						}
						this._speedEvents.Insert(i, @event);
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
		switch (Activator.CreateInstance(typeof(T), true)) // pattern matching sux
		{
			case InternalMoveEvent _: // those dont have side effect
				if (index <= this._moveEventIndex) this._moveEventIndex--;
				this._moveEvents.RemoveAt(index);
				break;
			case InternalRotationEvent _:
				if (index <= this._rotateEventIndex) this._rotateEventIndex--;
				this._rotateEvents.RemoveAt(index);
				break;
			case InternalOpacityEvent _:
				if (index <= this._opacityEventIndex) this._opacityEventIndex--;
				this._opacityEvents.RemoveAt(index);
				break;
			case InternalSpeedEvent _:
				if (index < this._speedEventIndex)
				{
					InternalSpeedEvent @event = this._speedEvents[index];
					this._elapsedNotePos -= @event.GetIntegral();
					this._speedEventIndex--;
				}
				if (index == this._speedEventIndex)
				{
					InternalSpeedEvent @event = this._speedEvents[index];
					this._elapsedNotePos -= @event.GetIntegral(rangeMax: (this._lastSpeedEventUpdateTimeMS - @event.StartMS) / (@event.EndMS - @event.StartMS));
					this._speedEventIndex--;
				}
				this._speedEvents.RemoveAt(index);
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
				foreach (var e in this._moveEvents)
				{
					@event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) break;
				}
				break;
			case InternalRotationEvent @event:
				foreach (var e in this._rotateEvents)
				{
					@event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) break;
				}
				break;
			case InternalOpacityEvent @event:
				foreach (var e in this._opacityEvents)
				{
					@event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) break;
				}
				break;
			case InternalSpeedEvent @event:
				foreach (var e in this._speedEvents)
				{
					@event.CheckOverlap(e);
					if (@event.EndTime < e.StartTime) break;
				}
				break;
			default:
				throw new Exception("Unregistered event!");
		}
	}
	[NotRecommended("Can cause performance issues!")]
	public List<T> GetEventListCopy<T>() where T : notnull, InternalEvent
	{
		List<T> list = new();
		switch (Activator.CreateInstance(typeof(T), true)) // pattern matching sux
		{
			case InternalMoveEvent _:
				foreach (var e in this._moveEvents) list.Add((T)e.QuickCopy());
				return list;
			case InternalRotationEvent _:
				foreach (var e in this._rotateEvents) list.Add((T)e.QuickCopy());
				return list;
			case InternalOpacityEvent _:
				foreach (var e in this._opacityEvents) list.Add((T)e.QuickCopy());
				return list;
			case InternalSpeedEvent _:
				foreach (var e in this._speedEvents) list.Add((T)e.QuickCopy());
				return list;
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
	private void RecheckTimeMSAll(BeatInfo onlyAfter)
	{
		foreach (var e in this._moveEvents)
		{
			if (e.EndTime < onlyAfter) continue;
			e.SetStartTime(in this._bpmEvents, e.StartTime);
			e.SetEndTime(in this._bpmEvents, e.EndTime);
		}
		foreach (var e in this._rotateEvents)
		{
			if (e.EndTime < onlyAfter) continue;
			e.SetStartTime(in this._bpmEvents, e.StartTime);
			e.SetEndTime(in this._bpmEvents, e.EndTime);
		}
		foreach (var e in this._opacityEvents)
		{
			if (e.EndTime < onlyAfter) continue;
			e.SetStartTime(in this._bpmEvents, e.StartTime);
			e.SetEndTime(in this._bpmEvents, e.EndTime);
		}
		foreach (var e in this._speedEvents)
		{
			if (e.EndTime < onlyAfter) continue;
			e.SetStartTime(in this._bpmEvents, e.StartTime);
			e.SetEndTime(in this._bpmEvents, e.EndTime);
		}
		foreach (var e in this._notes)
		{
			if (e.Time + e.HoldLength < onlyAfter) continue;
			e.SetTime(in this._bpmEvents, in this._speedEvents, e.Time); // set hold length is included in the method
		}
	}
	public InternalBPMEvent GetBpmEventValue(int index)
	{
		return (InternalBPMEvent)this._bpmEvents[index].QuickCopy();
	}
	public void AddBpmEvent(InternalBPMEvent @event, bool rebuildEverything = false)
	{
		this._bpmEvents.Add(@event);
		if (rebuildEverything)
		{
			this.ModifyEventTimeByBpmList();
		}
		BeatInfo info = BeatInfo.Zero;
		for (int i = 0; i < this._bpmEvents.Count - 1; i++)
		{
			info += this._bpmEvents[i].StartTimeRelativeToLast;
		}
		this.RecheckTimeMSAll(info);
	}
	public void RemoveBpmEvent(int index, bool maintainStructure = true, bool rebuildEverything = false)
	{
		if (maintainStructure && index != this._bpmEvents.Count && false) // kinda lazy to impl it now
		{
			var eventNow = this._bpmEvents[index];
			var eventNext = this._bpmEvents[index + 1];

			this._bpmEvents[index + 1].StartTimeRelativeToLast = eventNext.StartTimeRelativeToLast + eventNow.StartTimeRelativeToLast * new BeatInfo(0, (int)eventNext.Bpm, (int)eventNow.Bpm);
		}
		this._bpmEvents.RemoveAt(index);
		if (rebuildEverything) this.ModifyEventTimeByBpmList();
		BeatInfo info = BeatInfo.Zero;
		for (int i = 0; i < Math.Max(index - 1, 0); i++)
		{
			info += this._bpmEvents[i].StartTimeRelativeToLast;
		}
		this.RecheckTimeMSAll(info);
	}
	public void InsertBpmEvent(int index, InternalBPMEvent @event, bool rebuildEverything = false)
	{
		this._bpmEvents.Insert(index, @event);
		if (rebuildEverything) this.ModifyEventTimeByBpmList();
		BeatInfo info = BeatInfo.Zero;
		for (int i = 0; i < Math.Max(index - 1, 0); i++)
		{
			info += this._bpmEvents[i].StartTimeRelativeToLast;
		}
		this.RecheckTimeMSAll(info);
	}
	public void ModifyBpmEvent(int index, InternalBPMEvent @event, bool rebuildEverything = false)
	{
		this._bpmEvents[index] = @event;
		if (rebuildEverything) this.ModifyEventTimeByBpmList();
		BeatInfo info = BeatInfo.Zero;
		for (int i = 0; i < Math.Max(index - 1, 0); i++)
		{
			info += this._bpmEvents[i].StartTimeRelativeToLast;
		}
		this.RecheckTimeMSAll(info);
	}
	public ReadOnlyCollection<InternalBPMEvent> GetBpmListCopy() => this._bpmEvents.AsReadOnly();
	#endregion
}