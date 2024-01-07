namespace yt6983138.github.io.Components.WebPhiCharter;
public struct BeatInfo
{
    public required int Beat { get; set; }
    public required int DivisionNumerator { get; set; }
    public required int DivisionLevel { get; set; }

    public static BeatInfo Zero { get { return new() { Beat = 0, DivisionLevel = 0, DivisionNumerator = 0 }; } }

    public int GetMS(float bpm)
    {
        return (int)(bpm / 60 * ((float)this.Beat + (float)DivisionNumerator / (float)DivisionLevel) * 1000);
    }
    public int GetMS(List<(int ms, float bpm)> bpmEvents)
    {
        throw new NotImplementedException();
    }
}