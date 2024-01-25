namespace yt6983138.github.io.Components.WebPhiCharter;

public static class Misc
{
	public static CanvasHelper CanvasHelperHolder { get; set; }
	public static InternalChart ChartHolder { get; set; } = new();
	public static Logger PageLogger { get; set; } = new();
}
