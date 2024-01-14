using System.Diagnostics.CodeAnalysis;

namespace yt6983138.github.io.Components.WebPhiCharter;
public struct BeatInfo
{
	private int _numerator;
	public required int Beat { get; set; }
	public required int DivisionLevel { get; set; } = 1;

	public required int DivisionNumerator
	{
		get
		{
			return _numerator;
		}
		set
		{
			if (DivisionLevel != 0)
			{
				Beat += value / DivisionLevel; // int division
				_numerator = value % DivisionLevel;
			}
			else
			{
				_numerator = value;
			}
		}
	}

	public static BeatInfo Zero { get { return new() { Beat = 0, DivisionLevel = 0, DivisionNumerator = 0 }; } }

	public BeatInfo() { }
	public int GetMS(float bpm)
	{
		if (DivisionLevel == 0) return (int)(bpm / 60 * Beat * 1000);
		return (int)(bpm / 60 * ((float)this.Beat + (float)DivisionNumerator / (float)DivisionLevel) * 1000);
	}
	/// <summary>
	/// get ms with bpm events as input
	/// </summary>
	/// <returns>calculated ms</returns>
	public int GetMS(in List<InternalBPMEvent> bpmEvents)
	{
		int sum = 0;
		BeatInfo sum2 = Zero;
		for (int i = 0; i < bpmEvents.Count; i++)
		{
			InternalBPMEvent @event = bpmEvents[i];
			BeatInfo time = @event.StartTimeRelativeToLast;
			sum2 += time;
			sum += time.GetMS(@event.Bpm);
			BeatInfo delta = this - sum2;
			if (i + 1 == bpmEvents.Count || this < sum2 + bpmEvents[i + 1].StartTimeRelativeToLast) // 
			{
				sum2 += delta;
				sum += delta.GetMS(@event.Bpm);
				break;
			}
		}
		return sum;
	}
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		return base.Equals(obj);
	}
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
	public static bool operator <(BeatInfo first, BeatInfo second)
	{
		if (first.Beat < second.Beat) return true;
		if ((float)first.DivisionNumerator / first.DivisionLevel < (float)second.DivisionNumerator / second.DivisionLevel) return true;
		return false;
	}
	public static bool operator >(BeatInfo first, BeatInfo second)
	{
		return second < first;
	}
	public static bool operator ==(BeatInfo first, BeatInfo second)
	{
		return first.Equals(second);
	}
	public static bool operator !=(BeatInfo first, BeatInfo second)
	{
		return !first.Equals(second);
	}
	public static BeatInfo operator +(BeatInfo first, BeatInfo second)
	{
		BeatInfo tmp = Zero;
		tmp.Beat = first.Beat + second.Beat;
		int tmpDivision = first.DivisionLevel * second.DivisionLevel;
		int tmpFirstNumerator = first.DivisionNumerator * second.DivisionLevel;
		int tmpSecondNumerator = second.DivisionNumerator * first.DivisionLevel;
		int gcd = Utils.GCD(tmpDivision, tmpSecondNumerator + tmpFirstNumerator);
		gcd = (gcd == 0 ? 1 : gcd);
		tmp.DivisionLevel = tmpDivision / gcd;
		tmp.DivisionNumerator = (tmpFirstNumerator + tmpSecondNumerator) / gcd;

		return tmp;
	}
	public static BeatInfo operator -(BeatInfo first, BeatInfo second)
	{
		BeatInfo tmp = Zero;
		tmp.Beat = first.Beat - second.Beat;
		int tmpDivision = first.DivisionLevel * second.DivisionLevel;
		int tmpFirstNumerator = first.DivisionNumerator * second.DivisionLevel;
		int tmpSecondNumerator = second.DivisionNumerator * first.DivisionLevel;
		int i = tmpFirstNumerator - tmpSecondNumerator;
		while (i < 0)
		{
			i += tmpDivision;
			tmp.Beat--;
		}
		int gcd = Utils.GCD(tmpDivision, i);
		gcd = (gcd == 0 ? 1 : gcd);
		tmp.DivisionLevel = tmpDivision / gcd;
		tmp.DivisionNumerator = i / gcd;

		return tmp;
	}
	public static BeatInfo operator *(BeatInfo first, BeatInfo second)
	{
		(int n, int d) one = (first.DivisionNumerator * first.DivisionLevel, first.DivisionLevel);
		(int n, int d) two = (second.DivisionNumerator * second.DivisionLevel, second.DivisionLevel);
		(int n, int d) combined = (one.n * two.d + two.n * one.d, one.d * two.d);
		int gcd = Utils.GCD(combined.n, combined.d);
		return new BeatInfo() { Beat = 0, DivisionLevel = combined.d / gcd, DivisionNumerator = combined.n / gcd };
	}
}