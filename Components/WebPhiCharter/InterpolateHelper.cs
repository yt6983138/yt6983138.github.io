namespace yt6983138.github.io.Components.WebPhiCharter;

public static class InterpolateHelper
{
	/// <summary>
	/// interpolate function, x: 0 ~ 1
	/// </summary>
	/// <param name="mode"></param>
	/// <param name="x">0 ~ 1</param>
	/// <returns>float range 0 ~ 1</returns>
	public static float InterpolateSimpleInternal(InternalEventInterpolateMode mode, float x)
	{
		switch (mode)
		{
			case InternalEventInterpolateMode.Linear:
				return x;
			case InternalEventInterpolateMode.Log10: // convert x to range 0.5 ~ 1, after calculate convert back to 0 ~ 1
				return (float)Math.Log10(x * 0.5 + 0.5) * 3.32192809f + 1;
			case InternalEventInterpolateMode.Square:
				return x * x;
			case InternalEventInterpolateMode.Expo2:
				// same as above, just throw those shit into desmos to get graph
				// return ((float)Math.Pow(10, x - 1) - 1) * 1.111111111111111f + 1;
				return (float)(Math.Pow(2, 10 * x - 10));
			case InternalEventInterpolateMode.Sine:
				return (float)Math.Sin(x * Math.PI) * 0.5f;
			case InternalEventInterpolateMode.Cutoff:
				return x > 0 ? 1 : 0;
			case InternalEventInterpolateMode.Custom:
			default:
				throw new NotSupportedException("Use external call!");
		}
	}
	public static float Interpolate(InternalEventInterpolateMode mode, EaseMode easeMode, float start, float end, float x, string? customStr = null)
	{
		if (mode == InternalEventInterpolateMode.Custom)
		{
			throw new NotImplementedException();
		}
		float delta = end - start;
		float value = 0;
		if (easeMode == EaseMode.EaseIn)
		{
			value = InterpolateSimpleInternal(mode, x);
		}
		if (easeMode == EaseMode.EaseOut)
		{
			value = InterpolateSimpleInternal(mode, 1 - x) * -1 + 1;
		}
		if (easeMode == EaseMode.EaseBoth)
		{
			if (x > 0.5)
			{
				value = InterpolateSimpleInternal(mode, -2f * x + 2) * -1 - 1;
			} else
			{
				value = InterpolateSimpleInternal(mode, -2f * x) * 0.5f;
			}
		}
		return start + value * delta;
	}
}
