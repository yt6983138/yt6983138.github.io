﻿using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;

namespace yt6983138.github.io.Components.WebPhiCharter;

public partial class InternalJudgeLine
{
	public int GetNoteCount() => this._notes.Count;
	public InternalNote GetNoteValue(int index)
	{
		return (InternalNote)this._notes[index].QuickCopy();
	}
	public void AddNote(InternalNote note)
	{
		// todo: multi detection
		this._notes.Add(note);
	}
	public void RemoveNote(int index)
	{
		// todo: multi detection
		this._notes.RemoveAt(index);
	}
	[NotRecommended("Can cause performance issues!")]
	public List<InternalNote> GetNotesCopy()
	{
		return this._notes.ToList();
	}
}