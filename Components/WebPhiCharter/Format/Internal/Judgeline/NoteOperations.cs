using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;

namespace yt6983138.github.io.Components.WebPhiCharter;

public partial class InternalJudgeLine
{
	public int GetNoteCount() => _notes.Count;
	public InternalNote GetNoteValue(int index)
	{
		return (InternalNote)_notes[index].QuickCopy();
	}
	public void AddNote(InternalNote note)
	{
		// todo: multi detection
		_notes.Add(note);
	}
	public void RemoveNote(int index)
	{
		// todo: multi detection
		_notes.RemoveAt(index);
	}
	[NotRecommended("Can cause performance issues!")]
	public List<InternalNote> GetNotesCopy()
	{
		return _notes.ToList();
	}
}