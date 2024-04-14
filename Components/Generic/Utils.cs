namespace yt6983138.github.io;
public static class Utils
{
	public static int GCD(int a, int b)
	{
		if (a == 0)
		{
			return b;
		}
		if (b == 0)
		{
			return a;
		}

		int k;
		for (k = 0; ((a | b) & 1) == 0; ++k)
		{
			a >>= 1;
			b >>= 1;
		}

		while ((a & 1) == 0)
		{
			a >>= 1;
		}
		do
		{
			while ((b & 1) == 0)
			{
				b >>= 1;
			}
			if (a > b)
			{
				(b, a) = (a, b);
			}
			b -= a;
		} while (b != 0);

		return a << k;

	}
	/// <summary>
	/// Try to parse floats to fraction
	/// http://stackoverflow.com/questions/5124743/algorithm-for-simplifying-decimal-to-fractions/32903747#32903747
	/// </summary>
	/// <param name="value">any</param>
	/// <param name="accuracy">zero to one</param>
	/// <returns>Numerator and denominator</returns>
	/// <exception cref="ArgumentOutOfRangeException">thrown if accuracy > 1 or accuracy < 0</exception>
	public static (int Numerator, int Denominator) RealToFraction(double value, double accuracy)
	{
		if (accuracy <= 0.0 || accuracy >= 1.0)
		{
			throw new ArgumentOutOfRangeException(nameof(accuracy), "Must be > 0 and < 1.");
		}

		int sign = Math.Sign(value);

		if (sign == -1)
		{
			value = Math.Abs(value);
		}

		// Accuracy is the maximum relative error; convert to absolute maxError
		double maxError = sign == 0 ? accuracy : value * accuracy;

		int n = (int)Math.Floor(value);
		value -= n;

		if (value < maxError)
		{
			return (sign * n, 1);
		}

		if (1 - maxError < value)
		{
			return (sign * (n + 1), 1);
		}

		// The lower fraction is 0/1
		int lower_n = 0;
		int lower_d = 1;

		// The upper fraction is 1/1
		int upper_n = 1;
		int upper_d = 1;

		while (true)
		{
			// The middle fraction is (lower_n + upper_n) / (lower_d + upper_d)
			int middle_n = lower_n + upper_n;
			int middle_d = lower_d + upper_d;

			if (middle_d * (value + maxError) < middle_n)
			{
				// real + error < middle : middle is our new upper
				upper_n = middle_n;
				upper_d = middle_d;
			}
			else if (middle_n < (value - maxError) * middle_d)
			{
				// middle < real - error : middle is our new lower
				lower_n = middle_n;
				lower_d = middle_d;
			}
			else
			{
				// Middle is our best fraction
				return (((n * middle_d) + middle_n) * sign, middle_d);
			}
		}
	}
	public static float Range(float min, float num, float max)
	{
		return Math.Max(Math.Min(max, num), min);
	}
}
