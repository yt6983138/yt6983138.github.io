namespace yt6983138.github.io.Components.WebPhiCharter;

public enum InternalNoteType
{
	Tap,
	Hold,
	Drag,
	Flick,
	Custom
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