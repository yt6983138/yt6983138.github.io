﻿namespace yt6983138.github.io.RksReaderEnhanced;

public enum ScoreStatus
{
	Bugged = -1,
	NotFc = 0,
	Fc = 1,
	Phi = 2,
	Vu = 3,
	S = 4,
	A = 5,
	B = 6,
	C = 7,
	False = 8
}
public struct ScoreFormat
{
	/// <summary>
	/// Score, ex: 996105
	/// </summary>
	public int s;
	/// <summary>
	/// Acc, ex: 99.56718444824219
	/// </summary>
	public double a;
	/// <summary>
	/// Score Status, 0: not fc, 1: fc...
	/// </summary>
	public ScoreStatus c;
	public InternalScoreFormat ToInternalFormat(float chartConstant, string songName, string diffcultyName)
	{
		return new InternalScoreFormat { Score = this.s, Acc = this.a, Status = Helper.ParseStatus(this), ChartConstant = chartConstant, Name = songName, DifficultyName = diffcultyName };
	}
}
public struct InternalScoreFormat
{
	public int Score = 0;
	/// <summary>
	/// note: format: 99.x% = 99.xxxx...
	/// </summary>
	public double Acc = 0;
	public float ChartConstant = 0;
	public string Name = "Unset";
	public string DifficultyName = "Unset";
	public ScoreStatus Status = ScoreStatus.False;
	public InternalScoreFormat(int score, double acc, float chartConstant, string name, string diffcultyName, ScoreStatus status)
	{
		this.Score = score;
		this.Acc = acc;
		this.ChartConstant = chartConstant;
		this.Name = name;
		this.Status = status;
		this.DifficultyName = diffcultyName;
	}
	public double GetRksCalculated()
	{
		if (this.Acc < 0.7)
		{
			return 0;
		}
		return Math.Pow((this.Acc - 55) / 45, 2) * this.ChartConstant;
	}
	public ExportScoreFormat Export(string name)
	{
		return new ExportScoreFormat()
		{
			ID = this.Name,
			Name = name,
			Difficulty = this.DifficultyName,
			ChartConstant = this.ChartConstant,
			Score = this.Score,
			Acc = this.Acc,
			RksGiven = GetRksCalculated(),
			Stat = this.Status.ToString()
		};
	}
	public override string ToString()
	{
		return $"Score: {this.Score}, Acc: {this.Acc}, Status: {nameof(this.Status)}, cc: {this.ChartConstant}, calcedRks: {GetRksCalculated()}";
	}
}
public struct ExportScoreFormat
{
	// ID, Name, Difficulty, Chart Constant, Score, Acc, Rks Given, Stat
	public string ID;
	public string Name;
	public string Difficulty;
	public float ChartConstant;
	public int Score;
	public double Acc;
	public double RksGiven;
	public string Stat;
}