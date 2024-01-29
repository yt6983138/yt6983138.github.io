using System.Numerics;

namespace yt6983138.github.io.Components.WebPhiCharter;
public partial class InternalJudgeLine
{
    // please use public Events property if the event WILL BE CHANGED
    private List<InternalMoveEvent> _moveEvents = new();
    private List<InternalRotationEvent> _rotateEvents = new();
    private List<InternalOpacityEvent> _opacityEvents = new();
    private List<InternalSpeedEvent> _speedEvents = new();
    private List<InternalBPMEvent> _bpmEvents = new();

    private List<InternalNote> _notes = new();
    public Dictionary<int, InternalNoteAdditionalEvent> AdditionalEventForNote { get; set; } = new();

    public InternalJudgeLine(float baseBpm)
    {
		this._bpmEvents.Add(new InternalBPMEvent() { Bpm = baseBpm, StartTimeRelativeToLast = BeatInfo.Zero });
    }
}
