namespace yt6983138.github.io.Components.WebPhiCharter;

public enum InternalNoteType
{
	Tap = 1,
	Drag = 2,
	Hold = 3,
	Flick = 4,
	Custom = -1
}
public enum InternalEventInterpolateMode
{
	Linear, // x = y
	Log10, // x = log10(y)
	Square, // x = y^2
	Expo2, // x = 10^y
	Sine, // x = sin(y)
	Cutoff, // x = y > 0.5 ? 1 : 0
	Custom // do not support integral
}
public enum EaseMode
{
	EaseIn,
	EaseOut,
	EaseBoth
}