namespace yt6983138.github.io.Pages;

public partial class _114514ScoreGenerator
{
	#region Input
	public int NoteCount { get; set; }
	public int ScoreWanted { get; set; }
	#endregion

	#region Output
	public string CalculatedScore { get; private set; } = "";
	public int GoodRequired { get; private set; }
	public int PerfectRequired { get; private set; }
	public int ComboRequired { get; private set; }

	public string Status { get; private set; } = "Invalid Input!";
	#endregion

	public void Calculate()
	{
		if (NoteCount < 1 ||
			ScoreWanted < 0 ||
			ScoreWanted > 1000000)
		{
			Status = "Invalid Input!";
			return;
		}

		Status = "Calculating...";

		float perfectPointPerNote = 900000 / (float)NoteCount;
		float goodPointPerNote = perfectPointPerNote * 0.65f;

		for (int totalCount = 0; totalCount <= NoteCount; totalCount++)
		{
			if (totalCount * perfectPointPerNote > ScoreWanted + 1)
				goto Failed;

			for (int perfectCount = totalCount; perfectCount > 0; perfectCount--)
			{
				int goodCount = totalCount - perfectCount;
				float allPerfectScore = perfectPointPerNote * perfectCount;
				float allGoodScore = goodPointPerNote * goodCount;
				float noComboScore = allPerfectScore + allGoodScore;

				if (noComboScore < ScoreWanted - 100001)
					break;

				int estComboLess = (int)Utils.Range(1, (ScoreWanted - noComboScore) / 100000 * NoteCount - 1, totalCount);
				for (int combo = estComboLess; combo <= totalCount; combo++)
				{
					float comboScore = (float)combo / NoteCount * 100000;
					float withComboScore = noComboScore + comboScore;

					//if (withComboScore > 100000)
					//	Console.WriteLine(withComboScore );
					if (withComboScore > ScoreWanted + 1)
						break;

					if ((int)Math.Round(withComboScore, MidpointRounding.ToEven) == ScoreWanted)
					{
						CalculatedScore = withComboScore.ToString();
						GoodRequired = goodCount;
						PerfectRequired = perfectCount;
						ComboRequired = combo;

						Status = "Solution found!";
						return;
					}
				}
			}
		}
	Failed:
		Status = "No solution found!";
		return;
	}
}
