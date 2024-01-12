using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;

namespace yt6983138.github.io.Components.WebPhiCharter;

public partial class InternalJudgeLine
{
	#region Fields
	private int _moveEventIndex = 0;
	private int _rotateEventIndex = 0;
	private int _opacityEventIndex = 0;
	private int _speedEventIndex = 0;
	private int _lastSpeedEventUpdateTimeMS = 0;

	private int _bpmEventIndex = 0;

	private float _elapsedNotePos = 0;
	private float _currentOpacity = 0;
	private float _currentPosX = 0;
	private float _currentPosY = 0;
	private float _currentRotationEuler = 0;
	#endregion
	public void Update(ref BECanvasComponent component, ref Canvas2DContext context, int timeMS)
	{
		// iterate thru events
		#region Iterate events
		// speed event
		for (int i = _speedEventIndex; i < _speedEvents.Count; i++)
		{
			var @event = _speedEvents[i];
			int eventEndMS = @event.EndMS;
			int eventStartMS = @event.StartMS;
			int eventTimeDelta = eventEndMS - eventStartMS;

			int lastUpdateTimeDelta = _lastSpeedEventUpdateTimeMS - eventStartMS;
			int nowTimeDelta = timeMS - eventStartMS;

			if (timeMS > eventEndMS)
			{
				_elapsedNotePos += @event.GetIntegral((float)lastUpdateTimeDelta / eventTimeDelta, 1); // prevent event not ending correctly
				_lastSpeedEventUpdateTimeMS = eventEndMS;
				continue;
			}
			if (timeMS < eventStartMS) break;

			_elapsedNotePos += @event.GetIntegral((float)lastUpdateTimeDelta / eventTimeDelta, (float)nowTimeDelta / eventTimeDelta);
			// ^ use cast to prevent int division

			_lastSpeedEventUpdateTimeMS = timeMS;
			_speedEventIndex = i;
			break;
		}
		// opacity event
		for (int i = _opacityEventIndex; i < _opacityEvents.Count; i++)
		{
			var @event = _opacityEvents[i];
			int eventEndMS = @event.EndMS;
			int eventStartMS = @event.StartMS;
			int eventTimeDelta = eventEndMS - eventStartMS;

			int nowTimeDelta = timeMS - eventStartMS;

			if (timeMS > eventEndMS) continue; // restart loop directly
			if (timeMS < eventStartMS) break;

			_currentOpacity = InterpolateHelper.Interpolate(
				@event.InterpolateMode,
				@event.EaseMode,
				@event.OpacityStart,
				@event.OpacityEnd,
				nowTimeDelta / eventTimeDelta,
				@event.CustomInterpolateFunction
			);

			_opacityEventIndex = i;
			break;
		}
		// move event
		for (int i = _moveEventIndex; i < _moveEvents.Count; i++)
		{
			var @event = _moveEvents[i];
			int eventEndMS = @event.EndMS;
			int eventStartMS = @event.StartMS;
			int eventTimeDelta = eventEndMS - eventStartMS;

			int nowTimeDelta = timeMS - eventStartMS;

			if (timeMS > eventEndMS) continue; // restart loop directly
			if (timeMS < eventStartMS) break;

			_currentPosX = InterpolateHelper.Interpolate(
				@event.InterpolateModeX,
				@event.EaseModeX,
				@event.MovementStart.X,
				@event.MovementEnd.X,
				nowTimeDelta / eventTimeDelta,
				@event.CustomInterpolateFunctionX
			);
			_currentPosY = InterpolateHelper.Interpolate(
				@event.InterpolateModeY,
				@event.EaseModeY,
				@event.MovementStart.Y,
				@event.MovementEnd.Y,
				nowTimeDelta / eventTimeDelta,
				@event.CustomInterpolateFunctionY
			);

			_moveEventIndex = i;
			break;
		}
		// rotation event
		for (int i = _rotateEventIndex; i < _rotateEvents.Count; i++)
		{
			var @event = _rotateEvents[i];
			int eventEndMS = @event.EndMS;
			int eventStartMS = @event.StartMS;
			int eventTimeDelta = eventEndMS - eventStartMS;

			int nowTimeDelta = timeMS - eventStartMS;

			if (timeMS > eventEndMS) continue; // restart loop directly
			if (timeMS < eventStartMS) break;

			_currentRotationEuler = InterpolateHelper.Interpolate(
				@event.InterpolateMode,
				@event.EaseMode,
				@event.RotationStart,
				@event.RotationEnd,
				nowTimeDelta / eventTimeDelta,
				@event.CustomInterpolateFunction
			);

			_rotateEventIndex = i;
			break;
		}
		#endregion

		#region Draw judgeline
		// todo
		#endregion
	}
}