using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Text;
using System.Xml;
using yt6983138.github.io;
using yt6983138.github.io.RksReaderEnhanced;

namespace yt6983138.github.io.Pages;

public partial class RksReaderEnhanced : ComponentBase
{
	#region Datas
	private readonly static Dictionary<ScoreStatus, string> IconUrl = new()
	{
		{ ScoreStatus.Bugged, "/Assets/RksReader/Icons/QuestionMark.png" },
		{ ScoreStatus.NotFc, "/Assets/RksReader/Icons/QuestionMark.png" },
		{ ScoreStatus.Fc, "/Assets/RksReader/Icons/Fc.png" },
		{ ScoreStatus.Phi, "/Assets/RksReader/Icons/Phi.png" },
		{ ScoreStatus.Vu, "/Assets/RksReader/Icons/Vu.png" },
		{ ScoreStatus.S, "/Assets/RksReader/Icons/S.png" },
		{ ScoreStatus.A, "/Assets/RksReader/Icons/A.png" },
		{ ScoreStatus.B, "/Assets/RksReader/Icons/B.png" },
		{ ScoreStatus.C, "/Assets/RksReader/Icons/C.png" },
		{ ScoreStatus.False, "/Assets/RksReader/Icons/F.png" }
	};
	private const Int64 MaxFileSize = 1024 * 1024 * 16;
	#endregion

	#region Helpers
	private GoogleChartHelper? ChartHelper;
	private DownloadHelper? downloadHelper;
	private readonly static Logger PageLogger = new();
	private static SaveHelper? SaveHelper { get; set; }
	#endregion

	#region Settings
	public string ScoreStringFormat { get; set; } = "{0}";
	public string AccFormat { get; set; } = "{0:0.00}%";
	public string RksFormat { get; set; } = "{0:0.00}";
	public string ChartConstantFormat { get; set; } = "{0} {1:0.0}";
	public string DifficultyFileLocation { get; set; } = "/Assets/RksReader/3.4.2/difficulty.csv";
	public string NamesFileLocation { get; set; } = "/Assets/RksReader/3.4.2/info.csv";
	public string SessionToken { get; set; } = string.Empty;
	#endregion

	#region Temp datas
	private bool IsLoading = false;
	public bool Loaded { get; private set; } = false;
	private string CurrentAtt = "";
	public string SaveFileContent { get; private set; } = "";
	public Dictionary<string, string> RawParsedXml { get; private set; } = new();
	public Dictionary<string, string> DecryptedXml { get; private set; } = new();
	public List<InternalScoreFormat> AllScores { get; private set; } = new();
	public Dictionary<string, float[]> Difficulties { get; private set; } = new();
	public Dictionary<string, string> Names { get; private set; } = new();
	public Dictionary<string, (int ap, int fc, int vu, int s, int a, int b, int c, int f, int cleared)> Infos { get; set; } = new();
	#endregion

	#region misc
	private static void LogWithExThrown(LoggerType type, string message, Exception ex)
	{
		PageLogger.Log(type, $"Exception thrown! {message}, Exception:");
		PageLogger.Log(type, ex);
	}
	protected override void OnInitialized()
	{
		ChartHelper = new(JS);
		downloadHelper = new(JS);
		base.OnInitialized();
	}
	#endregion

	#region Sub-windows
	private enum SubWindowContent
	{
		None = -1,
		Log = 0,
		Export = 1,
		Graph = 2,
		CloudSave = 3
	}

	private SubWindowContent Content { get; set; } = SubWindowContent.None;
	private bool SubWindowOpen { get; set; } = false;

	private void CloseSubWindow()
	{
		Content = SubWindowContent.None;
		SubWindowOpen = false;
	}

	#region Chart
	private void InitChart()
	{
		ChartHelper!.Reset();
		ChartHelper!.Initalize("string", "Name", "number", "rks");
		ChartHelper!.CreateChart("Graph");
		int count = Math.Min(20, AllScores.Count);
		for (int i = 0; i < count; i++)
		{
			ChartHelper!.AddRow(
				(Names.TryGetValue(AllScores[i].Name, out string? value) ? value : AllScores[i].Name) + " " + AllScores[i].DifficultyName,
				AllScores[i].GetRksCalculated()
			);
		}
		var options = new
		{
			legend = "none",
			pointSize = 3,
			backgroundColor = "transparent",
			tooltip = new
			{
				isHtml = true
			},
			series = new object[]
			{
				new { color = "#fff" }
			},
			vAxis = new
			{
				maxValue = AllScores[1].GetRksCalculated(),
				minValue = AllScores[19].GetRksCalculated(),
				textStyle = new { color = "white" },
				gridlineColor = "#404040"
			},
			hAxis = new
			{
				textStyle = new { color = "white" }
			},
			chartArea = new
			{
				width = "90%",
				height = "70%",
				top = 10,
				right = 0
			}
		};

		ChartHelper.Draw(options);
		//ChartHelper.Draw(JsonConvert.DeserializeObject("{\"hAxis\":{\"title\":\"Horizontal Axis Label\"},\"vAxis\":{\"title\":\"Vertical Axis Label\"},\"title\":\"This is a Google Chart in Blazor\",\"legend\":{\"position\":\"none\"}}"));
	}
	#endregion
	#endregion

	#region Export
	private int CountToExport { get; set; } = 0;

	private async void ExportJSON()
	{
		int count = (CountToExport < 1) ? AllScores.Count : Math.Min(CountToExport, AllScores.Count);
		List<ExportScoreFormat> sliced = new();
		for (int i = 0; i < count; i++)
		{
			sliced.Add(AllScores[i].Export(Names.TryGetValue(AllScores[i].Name, out string? value) ? value : "Unknown"));
		}
		string content = JsonConvert.SerializeObject(sliced, Newtonsoft.Json.Formatting.Indented);
		await downloadHelper!.DownloadFromByte(Encoding.UTF8.GetBytes(content), "Export.json", "application/json");
	}
	private async void ExportCSV()
	{
		CsvBuilder builder = new();
		builder.AddHeader("ID", "Name", "Difficulty", "Chart Constant", "Score", "Acc", "Rks Given", "Stat");
		int count = (CountToExport < 1) ? AllScores.Count : Math.Min(CountToExport, AllScores.Count);
		for (int i = 0; i < count; i++)
		{
			string realName = Names.TryGetValue(AllScores[i].Name, out string? value) ? value : "Unknown";
			builder.AddRow(
				AllScores[i].Name,
				realName,
				AllScores[i].DifficultyName,
				AllScores[i].ChartConstant.ToString(),
				AllScores[i].Score.ToString(),
				AllScores[i].Acc.ToString(),
				AllScores[i].GetRksCalculated().ToString(),
				AllScores[i].Status.ToString()
			);
		}
		await downloadHelper!.DownloadFromByte(Encoding.UTF8.GetBytes(builder.Compile()), "Export.csv", "text/csv");
	}
	private async void ExportEncryptDataTable()
	{
		int i = 0;
		CsvBuilder builder = new();
		builder.AddHeader("Key encrypted", "Key decrypted", "Value encrypted", "Value decrypted");
		foreach (var pair in RawParsedXml)
		{
			try
			{
				string keyDecoded = System.Net.WebUtility.UrlDecode(pair.Key);
				string keyDecrypted = SaveHelper.DecryptSaveStringNew(keyDecoded);
				string valueDecoded = System.Net.WebUtility.UrlDecode(pair.Value);
				string valueDecrypted = SaveHelper.DecryptSaveStringNew(valueDecoded);
				builder.AddRow(keyDecoded, keyDecrypted, valueDecoded, valueDecrypted);
				i++;
			}
			catch { }
		}
		await downloadHelper!.DownloadFromByte(Encoding.UTF8.GetBytes(builder.Compile()), "ExportTable.csv", "text/csv");
	}
	#endregion

	#region Render and calculate
	public double AverageRks
	{
		get
		{
			double _rks = 0;
			for (int i = 0; i < Math.Min(20, AllScores.Count); i++)
			{
				_rks += AllScores[i].GetRksCalculated() * 0.05;
			}
			return _rks;
		}
	}

	private void Reset()
	{
		CurrentAtt = "";
		RawParsedXml.Clear();
		DecryptedXml.Clear();
		//AllScores.Clear();
		Difficulties.Clear();
		Names.Clear();
		AllScores.Clear();
		Loaded = false;
	}
	private void RenderAll()
	{
		Infos = new Dictionary<string, (int ap, int fc, int vu, int s, int a, int b, int c, int f, int cleared)>()
		{
			{ "EZ", new() },
			{ "HD", new() },
			{ "IN", new() },
			{ "AT", new() }
		};
		(int index, InternalScoreFormat score) highest = new();
		PageLogger.Log(LoggerType.Info, "Sorting Save...");
		int i = 0;
		AllScores.Sort((x, y) => y.GetRksCalculated().CompareTo(x.GetRksCalculated()));
		foreach (var score in AllScores)
		{
			if (score.GetRksCalculated() > highest.score.GetRksCalculated() && score.Acc == 100)
			{
				highest.index = i;
				highest.score = score;
			}
			i++;
			try
			{
				var _info = Infos[score.DifficultyName.ToUpper()];
				switch (score.Status)
				{
					case ScoreStatus.Phi:
						_info.ap++;
						goto case ScoreStatus.Fc;
					case ScoreStatus.Fc:
						_info.fc++;
						goto case ScoreStatus.NotFc;
					case ScoreStatus.Vu:
						_info.vu++;
						goto case ScoreStatus.NotFc;
					case ScoreStatus.S:
						_info.s++;
						goto case ScoreStatus.NotFc;
					case ScoreStatus.A:
						_info.a++;
						goto case ScoreStatus.NotFc;
					case ScoreStatus.B:
						_info.b++;
						goto case ScoreStatus.NotFc;
					case ScoreStatus.C:
						_info.c++;
						goto case ScoreStatus.NotFc;
					case ScoreStatus.False:
						_info.f++;
						goto case ScoreStatus.NotFc;
					case ScoreStatus.NotFc:
						_info.cleared++;
						break;
				}
				Infos[score.DifficultyName.ToUpper()] = _info;
			}
			catch { }
		}
		//Scores.Sort((x, y) => x.GetRksCalculated().CompareTo(y.GetRksCalculated()));
		PageLogger.Log(LoggerType.Info, "Sorting Save...");
		AllScores.Insert(0, highest.score);
		AllScores.Sort((x, y) => y.GetRksCalculated().CompareTo(x.GetRksCalculated()));
		AllScores.MoveItemAtIndexToFront(highest.index);
	}
	#endregion

	#region Cloud save
	private async void OnGetSaveByToken()
	{
		if (IsLoading || Loaded) { return; }
		IsLoading = true;
		try
		{
			SaveHelper = new(JS);
			SaveHelper.InitializeCloudHelper(SessionToken);
		} catch
		{
			PageLogger.Log(LoggerType.Error, "Invalid token!");
			IsLoading = false;
			return;
		}
		PageLogger.Log(LoggerType.Info, "Loading CSVs...");
		await LoadCSVs();
		PageLogger.Log(LoggerType.Info, "Loading Save From Remote...");
		var saves = await SaveHelper.GetGameSaves(Difficulties);
		Console.WriteLine(saves.Count);
		AllScores = saves[^1].Records; // latest
		IsLoading = false;
		Loaded = true;
		RenderAll();
		PageLogger.Log(LoggerType.Info, "Done Loading!");
	}
	#endregion

	#region Offline save
	private async Task OnLoadSave(InputFileChangeEventArgs e)
	{
		if (IsLoading || Loaded) { return; }
		IsLoading = true;
		try
		{
			PageLogger.Log(LoggerType.Info, "Reading File...");
			using (var reader = new StreamReader(e.File.OpenReadStream(MaxFileSize), System.Text.Encoding.UTF8))
			{
				SaveFileContent = await reader.ReadToEndAsync();
			}
			PageLogger.Log(LoggerType.Info, "Creating reader...");
			XmlReader xmlReader = XmlReader.Create(new StringReader(SaveFileContent)); // cant use e.File.OpenReadStream
			PageLogger.Log(LoggerType.Info, "Parsing XML...");
			while (xmlReader.Read())
			{
				switch (xmlReader.NodeType)
				{
					case XmlNodeType.Element:
						if (xmlReader.AttributeCount < 1) { break; }
						CurrentAtt = xmlReader.GetAttribute(0);
						break;
					case XmlNodeType.Text:
						RawParsedXml.Add(CurrentAtt, xmlReader.Value);
						break;
					case XmlNodeType.EndElement:
						break;
					default:
						break; // ignore
				}
			}
		}
		catch (Exception ex)
		{
			PageLogger.Log(LoggerType.Error, ex);
		}
		PageLogger.Log(LoggerType.Info, "Decrypting Save...");
		DecryptSave();
		PageLogger.Log(LoggerType.Info, "Loading CSVs...");
		await LoadCSVs();
		// done load csv
		PageLogger.Log(LoggerType.Info, "Filtering Save...");
		FilterSave();
		IsLoading = false;
		Loaded = true;
		RenderAll();
		PageLogger.Log(LoggerType.Info, "Done Loading!");
	}
	public void DecryptSave()
	{
		foreach (var pair in RawParsedXml)
		{
			if (System.Net.WebUtility.UrlDecode(pair.Key).Length % 4 != 0) { continue; }
			try
			{
				DecryptedXml.Add(SaveHelper.DecryptSaveStringNew(System.Net.WebUtility.UrlDecode(pair.Key)), SaveHelper.DecryptSaveStringNew(System.Net.WebUtility.UrlDecode(pair.Value)));
			}
			catch (Exception ex)
			{
				LogWithExThrown(LoggerType.Error, $"Processing {pair.Key}, {pair.Value}", ex);
			}
		}
	}
	public async Task LoadCSVs()
	{
		string[] csvFile = (await Http.GetStringAsync(DifficultyFileLocation)).Replace("\r", "").Split("\n");
		foreach (string line in csvFile)
		{
			try
			{
				float[] diffcultys = new float[4];
				string[] splitted = line.Split(",");
				for (byte i = 0; i < splitted.Length; i++)
				{
					if (i > 4 || i == 0) { continue; }
					if (!float.TryParse(splitted[i], out diffcultys[i - 1])) { Console.WriteLine($"Error processing {splitted[i]}"); }
				}
				// Console.WriteLine($"{splitted[0]}, {diffcultys[0]}, {diffcultys[1]}, {diffcultys[2]}, {diffcultys[3]}");
				Difficulties.Add(splitted[0], diffcultys);
			}
			catch (Exception ex)
			{
				PageLogger.Log(LoggerType.Error, ex);
			}
		}
		string[] csvFile2 = (await Http.GetStringAsync(NamesFileLocation)).Replace("\r", "").Split("\n");
		foreach (string line in csvFile2)
		{
			try
			{
				string[] splitted = line.Split(@"\");
				Names.Add(splitted[0], splitted[1]);
			}
			catch (Exception ex)
			{
				PageLogger.Log(LoggerType.Error, ex);
			}
		}
	}
	public void FilterSave()
	{
		foreach (var pair in DecryptedXml)
		{
			if (pair.Key.Split('.').Length < 4)
			{
				continue;
			}
			try
			{
				string[] splitted = pair.Key.Split(".");
				string id = $"{splitted[0]}.{splitted[1]}";
				AllScores.Add(
					JsonConvert.DeserializeObject<ScoreFormat>(DecryptedXml[pair.Key])
						.ToInternalFormat(
							Difficulties[id][Helper.DifficultStringToIndex(splitted[^1])],
							id,
							splitted[^1]
						)
				);
			}
			catch (Exception ex)
			{
				LogWithExThrown(LoggerType.Error, $"Processing {pair.Key}, {pair.Value}", ex);
			}
		}
	}
	#endregion
}