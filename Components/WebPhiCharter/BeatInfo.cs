using System.Diagnostics.CodeAnalysis;

namespace yt6983138.github.io.Components.WebPhiCharter;
public class BeatInfo
{
	private int _numerator;
	public int Beat { get; set; }
	public int DivisionLevel { get; set; } = 1;

	public int DivisionNumerator
	{
		get
		{
			return this._numerator;
		}
		set
		{
			if (this.DivisionLevel != 0)
			{
				this.Beat += value / this.DivisionLevel; // int division
				this._numerator = value % this.DivisionLevel;
			}
			else
			{
				this._numerator = value;
			}
		}
	}

	public static BeatInfo Zero { get { return new(0, 0, 0); } }

	private BeatInfo() { }
	public BeatInfo(int beat, int level, int numerator)
	{
		this.Beat = beat;
		this.DivisionLevel = level;
		this.DivisionNumerator = numerator;
	}
	public int GetMS(float bpm)
	{
		if (this.DivisionLevel == 0) return (int)(bpm / 60 * this.Beat * 1000);
		return (int)(bpm / 60 * ((float)this.Beat + (float)this.DivisionNumerator / (float)this.DivisionLevel) * 1000);
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
	public static bool operator ==(BeatInfo? first, BeatInfo? second)
	{
		if (first is null)
		{
			if (second is null) return true;
			return false;
		}
		return first.Equals(second);
	}
	public static bool operator !=(BeatInfo? first, BeatInfo? second)
	{
		return !(first == second);
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
		return new BeatInfo(0, combined.d / gcd, combined.n / gcd);
	}
}