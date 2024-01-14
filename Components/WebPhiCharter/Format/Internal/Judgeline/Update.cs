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
	public async void Update(int timeMS)
	{
		// iterate thru events
		#region Iterate events
		// reset if go back
		if (timeMS < _lastSpeedEventUpdateTimeMS)
		{
			_speedEventIndex = 0;
			_lastSpeedEventUpdateTimeMS = 0;
			_elapsedNotePos = 0;
			_moveEventIndex = 0;
			_rotateEventIndex = 0;
			_opacityEventIndex = 0;
		}

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

		Misc.CanvasHelperHolder.Save(); // bruh no ref in async methods

		#region Draw judgeline
		(float width, float height) = await Misc.CanvasHelperHolder.GetCanvasSize();

		float lineLength = width * TextureManager.LineBaseLength;
		float lineHeight = TextureManager.JudgeLineSize.Height * (TextureManager.JudgeLineSize.Width / lineLength);
		Misc.CanvasHelperHolder.GlobalAlpha = _currentOpacity;            // ^ calculate scale
		Misc.CanvasHelperHolder.Translate(_currentPosX * width, _currentPosY * height);
		Misc.CanvasHelperHolder.RotateEuler(_currentRotationEuler);
		Misc.CanvasHelperHolder.DrawImage(
			"JudgeLine",
			-0.5f * TextureManager.JudgeLineSize.Width,
			-0.5f * TextureManager.JudgeLineSize.Height,
			lineLength,
			lineHeight
		);

		Misc.CanvasHelperHolder.GlobalAlpha = 1;
		#endregion

		#region Draw notes
		foreach (var note in _notes)
		{
			if (note.TimeMS + 512 < timeMS) continue;

			InternalNoteAdditionalEvent additionalEvent = null!;
			if (note.ID != null) AdditionalEventForNote.TryGetValue((int)note.ID, out additionalEvent!);
			if (additionalEvent == null) additionalEvent = new InternalNoteAdditionalEvent();

			if (additionalEvent.VisibleSinceMS < timeMS && timeMS < additionalEvent.VisibleSinceMS + additionalEvent.VisibleTime)
			{
				//Misc.CanvasHelperHolder.GlobalAlpha = Utils.Range(
				//	0,
				//	InterpolateHelper.InterpolateSimpleInternal(
				//		InternalEventInterpolateMode.Linear, 
				//		(timeMS - note.TimeMS) / 512f), 
				//	1
				//) * additionalEvent.Opacity; // on miss only!
				Misc.CanvasHelperHolder.GlobalAlpha = note.TimeMS > timeMS ? 1 : 0;
			}
			else
			{
				Misc.CanvasHelperHolder.GlobalAlpha = 0;
			}
			Misc.CanvasHelperHolder.SetTransform(additionalEvent.RenderTransform);
			Misc.CanvasHelperHolder.Scale(1, note.IsDownSide ? 1 : -1);

			InternalNoteType type = additionalEvent.Texture.type ?? note.NoteType;
			bool isMulti = additionalEvent.Texture.multitap ?? false; // todo multi check

			float elapsedDelta = note.PosX - _elapsedNotePos;
			float noteWidth = TextureManager.NoteBaseScale * width;
			float noteScale = noteWidth / TextureManager.TapSize.Width;

			if (type == InternalNoteType.Custom)
			{
				if (additionalEvent.CustomTexturePath == null)
				{
					type = note.NoteType; // fall back
				}
				else
				{
					// todo
				}
			}
			if (type == InternalNoteType.Hold)
			{
				string head = isMulti ? "MultiHoldHead" : "HoldHead";
				(float Width, float Height) headSize = isMulti ? TextureManager.MultiHoldHeadSize : TextureManager.HoldHeadSize;
				float headScale = height / headSize.Height;
				string body = isMulti ? "MultiHoldBody" : "HoldBody";
				(float Width, float Height) bodySize = isMulti ? TextureManager.MultiHoldBodySize : TextureManager.HoldBodySize;
				float bodyScale = height / bodySize.Height;
				string end = isMulti ? "MultiHoldEnd" : "HoldEnd";
				(float Width, float Height) endSize = isMulti ? TextureManager.MultiHoldEndSize : TextureManager.HoldEndSize;
				float endScale = height / endSize.Height;

				float headCutHeight = Utils.Range(0, headScale * elapsedDelta * height, headSize.Height);
				if (headCutHeight < headSize.Height) // draw head
				{
					Misc.CanvasHelperHolder.DrawImage(
						head,
						0,
						0,
						headSize.Width,
						headCutHeight,
						note.PosX * width - headSize.Width * noteScale / 2 * additionalEvent.Scale.X * 1, // <- todo: note scale configuration
						0,
						headSize.Width * noteScale * additionalEvent.Scale.X * 1,
						(headSize.Height - headCutHeight) * noteScale * additionalEvent.Scale.Y * 1
					);
				}
				float bodyCutHeight = Utils.Range(0, bodyScale * elapsedDelta * height, bodySize.Height);
				if (bodyCutHeight < bodySize.Height) // draw body
				{
					Misc.CanvasHelperHolder.DrawImage(
						body,
						0,
						0,
						bodySize.Width,
						bodyCutHeight,
						note.PosX * width - bodySize.Width * noteScale / 2 * additionalEvent.Scale.X * 1, // <- todo: note scale configuration
						(headSize.Height - headCutHeight) * noteScale * additionalEvent.Scale.Y * 1, // to not override head
						bodySize.Width * noteScale * additionalEvent.Scale.X * 1,
						note.HoldRenderLength * height
					);
				}
				float endCutHeight = Utils.Range(0, endScale * elapsedDelta * height, endSize.Height);
				if (endCutHeight < endSize.Height) // draw end
				{
					Misc.CanvasHelperHolder.DrawImage(
						end,
						0,
						0,
						endSize.Width,
						endCutHeight,
						note.PosX * width - endSize.Width * noteScale / 2 * additionalEvent.Scale.X * 1, // <- todo: note scale configuration
						((headSize.Height - headCutHeight) * additionalEvent.Scale.Y * 1 + note.HoldRenderLength * height) * noteScale, // to not override head and body
						endSize.Width * noteScale * additionalEvent.Scale.X * 1,
						(endSize.Height - endCutHeight) * noteScale * additionalEvent.Scale.Y * 1
					);
				}
			}
			else
			{
				(float width, float height) noteSize;
				(string name, noteSize.width, noteSize.height) = TextureManager.GetTextureNonHold(type, isMulti);

				Misc.CanvasHelperHolder.DrawImage(
					name,
					0,
					0,
					noteSize.width,
					noteSize.height,
					note.PosX * width - noteSize.width * noteScale / 2 * additionalEvent.Scale.X * 1,
					note.PosY * height - _elapsedNotePos - noteSize.height * noteScale / 2,
					noteSize.width * noteScale,
					noteSize.height * noteScale
				);
			}

		}
		#endregion

		Misc.CanvasHelperHolder.Restore();
	}
}