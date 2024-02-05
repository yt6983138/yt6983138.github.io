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
	public string DifficultyFileLocation { get; set; } = "/Assets/RksReader/3.4.3/difficulty.csv";
	public string NamesFileLocation { get; set; } = "/Assets/RksReader/3.4.3/info.csv";
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

	public Summary CurrentSummary { get; set; } = Summary.Default;
	public UserInfo CurrentUserInfo { get; set; } = new();
	public List<(Summary Summary, GameSave Save)> CloudSaves { get; set; } = new();
	#endregion

	#region misc
	private static void LogWithExThrown(LoggerType type, string message, Exception ex)
	{
		PageLogger.Log(type, $"Exception thrown! {message}, Exception:");
		PageLogger.Log(type, ex);
	}
	protected override void OnInitialized()
	{
		this.ChartHelper = new(this.JS);
		this.downloadHelper = new(this.JS);
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
		this.Content = SubWindowContent.None;
		this.SubWindowOpen = false;
	}

	#region Chart
	private void InitChart()
	{
		this.ChartHelper!.Reset();
		this.ChartHelper!.Initalize("string", "Name", "number", "rks");
		this.ChartHelper!.CreateChart("Graph");
		int count = Math.Min(20, this.AllScores.Count);
		for (int i = 0; i < count; i++)
		{
			this.ChartHelper!.AddRow(
				(this.Names.TryGetValue(this.AllScores[i].Name, out string? value) ? value : this.AllScores[i].Name) + " " + this.AllScores[i].DifficultyName,
				this.AllScores[i].GetRksCalculated()
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
				maxValue = this.AllScores[1].GetRksCalculated(),
				minValue = this.AllScores[19].GetRksCalculated(),
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

		this.ChartHelper.Draw(options);
		//ChartHelper.Draw(JsonConvert.DeserializeObject("{\"hAxis\":{\"title\":\"Horizontal Axis Label\"},\"vAxis\":{\"title\":\"Vertical Axis Label\"},\"title\":\"This is a Google Chart in Blazor\",\"legend\":{\"position\":\"none\"}}"));
	}
	#endregion
	#endregion

	#region Export
	private int CountToExport { get; set; } = 0;

	private async void ExportJSON()
	{
		int count = (this.CountToExport < 1) ? this.AllScores.Count : Math.Min(this.CountToExport, this.AllScores.Count);
		List<ExportScoreFormat> sliced = new();
		for (int i = 0; i < count; i++)
		{
			sliced.Add(this.AllScores[i].Export(this.Names.TryGetValue(this.AllScores[i].Name, out string? value) ? value : "Unknown"));
		}
		string content = JsonConvert.SerializeObject(sliced, Newtonsoft.Json.Formatting.Indented);
		await this.downloadHelper!.DownloadFromByte(Encoding.UTF8.GetBytes(content), "Export.json", "application/json");
	}
	private async void ExportCSV()
	{
		CsvBuilder builder = new();
		builder.AddHeader("ID", "Name", "Difficulty", "Chart Constant", "Score", "Acc", "Rks Given", "Stat");
		int count = (this.CountToExport < 1) ? this.AllScores.Count : Math.Min(this.CountToExport, this.AllScores.Count);
		for (int i = 0; i < count; i++)
		{
			string realName = this.Names.TryGetValue(this.AllScores[i].Name, out string? value) ? value : "Unknown";
			builder.AddRow(
				this.AllScores[i].Name,
				realName,
				this.AllScores[i].DifficultyName,
				this.AllScores[i].ChartConstant.ToString(),
				this.AllScores[i].Score.ToString(),
				this.AllScores[i].Acc.ToString(),
				this.AllScores[i].GetRksCalculated().ToString(),
				this.AllScores[i].Status.ToString()
			);
		}
		await this.downloadHelper!.DownloadFromByte(Encoding.UTF8.GetBytes(builder.Compile()), "Export.csv", "text/csv");
	}
	private async void ExportEncryptDataTable()
	{
		int i = 0;
		CsvBuilder builder = new();
		builder.AddHeader("Key encrypted", "Key decrypted", "Value encrypted", "Value decrypted");
		foreach (var pair in this.RawParsedXml)
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
		await this.downloadHelper!.DownloadFromByte(Encoding.UTF8.GetBytes(builder.Compile()), "ExportTable.csv", "text/csv");
	}
	#endregion

	#region Render and calculate
	public double AverageRks
	{
		get
		{
			double _rks = 0;
			for (int i = 0; i < Math.Min(20, this.AllScores.Count); i++)
			{
				_rks += this.AllScores[i].GetRksCalculated() * 0.05;
			}
			return _rks;
		}
	}

	private void Reset()
	{
		this.CurrentAtt = "";
		this.RawParsedXml.Clear();
		this.DecryptedXml.Clear();
		//AllScores.Clear();
		this.Difficulties.Clear();
		this.Names.Clear();
		this.AllScores.Clear();
		this.CloudSaves.Clear();
		this.Loaded = false;
	}
	private void RenderAll()
	{
		this.Infos = new Dictionary<string, (int ap, int fc, int vu, int s, int a, int b, int c, int f, int cleared)>()
		{
			{ "EZ", new() },
			{ "HD", new() },
			{ "IN", new() },
			{ "AT", new() }
		};
		(int index, InternalScoreFormat score) highest = (0, new()
		{
			Acc = 0,
			Score = 0,
			ChartConstant = 0,
			DifficultyName = "EZ",
			Name = "None",
			Status = ScoreStatus.Bugged
		});
		PageLogger.Log(LoggerType.Info, "Sorting Save...");
		int i = 0;
		this.AllScores.Sort((x, y) => y.GetRksCalculated().CompareTo(x.GetRksCalculated()));
		foreach (var score in this.AllScores)
		{
			if (score.GetRksCalculated() > highest.score.GetRksCalculated() && score.Acc == 100)
			{
				highest.index = i;
				highest.score = score;
			}
			i++;
			try
			{
				var _info = this.Infos[score.DifficultyName.ToUpper()];
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
				this.Infos[score.DifficultyName.ToUpper()] = _info;
			}
			catch { }
		}
		//Scores.Sort((x, y) => x.GetRksCalculated().CompareTo(y.GetRksCalculated()));
		PageLogger.Log(LoggerType.Info, "Sorting Save...");
		// this.AllScores.Sort((x, y) => y.GetRksCalculated().CompareTo(x.GetRksCalculated()));
		this.AllScores.Insert(0, highest.score);
		//this.AllScores.MoveItemAtIndexToFront(highest.index);
		base.StateHasChanged();
	}
	#endregion

	#region Cloud save
	private string _currentSelected = "";
	public string CurrentSelected
	{
		get => this._currentSelected;
		set
		{
			this._currentSelected = value;
			this.ChangeScores();
		}
	}
	public int MaxCloudSaveEntries { get; set; } = 3;

	public void ChangeScores()
	{
		if (this.IsLoading || !this.Loaded || !int.TryParse(this._currentSelected, out int value)) return;
		this.AllScores = new List<InternalScoreFormat>(this.CloudSaves[value].Save.Records);
		this.CurrentSummary = this.CloudSaves[value].Summary;
		RenderAll(); // ^ to prevent modification to record list
	}
	private async void OnGetSaveByToken()
	{
		if (this.IsLoading || this.Loaded) { return; }
		this.IsLoading = true;
		try
		{
			SaveHelper = new() { Runtime = this.JS };
			SaveHelper.InitializeCloudHelper(this.SessionToken);
		}
		catch
		{
			PageLogger.Log(LoggerType.Error, "Invalid token!");
			this.IsLoading = false;
			return;
		}
		try
		{
			PageLogger.Log(LoggerType.Info, "Loading CSVs...");
			await this.LoadCSVs();
			PageLogger.Log(LoggerType.Info, "Loading Save From Remote...");
			this.CloudSaves = await SaveHelper.GetGameSaves(this.Difficulties, MaxCloudSaveEntries < 1 ? int.MaxValue : MaxCloudSaveEntries);
			this.CurrentUserInfo = await SaveHelper.GetUserInfo();
			// Console.WriteLine(CloudSaves.Count);
		}
		catch (Exception ex)
		{
			PageLogger.Log(LoggerType.Error, ex);
			this.IsLoading = false;
			this.Loaded = false;
			return;
		}
		this.IsLoading = false;
		this.Loaded = true;
		this.CurrentSelected = "0";
		// this.RenderAll();
		PageLogger.Log(LoggerType.Info, "Done Loading!");
	}
	#endregion

	#region Offline save
	private async Task OnLoadSave(InputFileChangeEventArgs e)
	{
		if (this.IsLoading || this.Loaded) { return; }
		this.IsLoading = true;
		try
		{
			PageLogger.Log(LoggerType.Info, "Reading File...");
			using (var reader = new StreamReader(e.File.OpenReadStream(MaxFileSize), System.Text.Encoding.UTF8))
			{
				this.SaveFileContent = await reader.ReadToEndAsync();
			}
			PageLogger.Log(LoggerType.Info, "Creating reader...");
			XmlReader xmlReader = XmlReader.Create(new StringReader(this.SaveFileContent)); // cant use e.File.OpenReadStream
			PageLogger.Log(LoggerType.Info, "Parsing XML...");
			while (xmlReader.Read())
			{
				switch (xmlReader.NodeType)
				{
					case XmlNodeType.Element:
						if (xmlReader.AttributeCount < 1) { break; }
						this.CurrentAtt = xmlReader.GetAttribute(0);
						break;
					case XmlNodeType.Text:
						this.RawParsedXml.Add(this.CurrentAtt, xmlReader.Value);
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
		this.DecryptSave();
		PageLogger.Log(LoggerType.Info, "Loading CSVs...");
		await this.LoadCSVs();
		// done load csv
		PageLogger.Log(LoggerType.Info, "Filtering Save...");
		this.FilterSave();
		this.CurrentUserInfo = new()
		{
			UserName = DecryptedXml.TryGetValue("playerID", out string? str1) ? str1 : "*Unknown*",
			ModificationTime = DecryptedXml.TryGetValue("saveBaseTime", out string? str2) ? DateTime.Parse(str2) : DateTime.UnixEpoch,
			CreationTime = DateTime.UnixEpoch,
			NickName = "*Unknown*"
		};
		this.CurrentSummary = new()
		{
			Avatar = string.Empty,
			ChallengeCode = DecryptedXml.TryGetValue("ChallengeModeRank", out string? str3) ? ushort.Parse(str3) : (ushort)0,
			GameVersion = -1,
			SaveVersion = -1,
			Clears = new()
		};
		this.RenderAll();
		this.IsLoading = false;
		this.Loaded = true;
		PageLogger.Log(LoggerType.Info, "Done Loading!");
	}
	public void DecryptSave()
	{
		foreach (var pair in this.RawParsedXml)
		{
			if (System.Net.WebUtility.UrlDecode(pair.Key).Length % 4 != 0) { continue; }
			try
			{
				this.DecryptedXml.Add(
					SaveHelper.DecryptSaveStringNew(System.Net.WebUtility.UrlDecode(pair.Key)),
					SaveHelper.DecryptSaveStringNew(System.Net.WebUtility.UrlDecode(pair.Value))
				);
			}
			catch (Exception ex)
			{
				LogWithExThrown(LoggerType.Error, $"Processing {pair.Key}, {pair.Value}", ex);
			}
		}
	}
	public async Task LoadCSVs()
	{
		string[] csvFile = (await this.Http.GetStringAsync(this.DifficultyFileLocation)).Replace("\r", "").Split("\n");
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
				this.Difficulties.Add(splitted[0], diffcultys);
			}
			catch (Exception ex)
			{
				PageLogger.Log(LoggerType.Error, ex);
			}
		}
		string[] csvFile2 = (await this.Http.GetStringAsync(this.NamesFileLocation)).Replace("\r", "").Split("\n");
		foreach (string line in csvFile2)
		{
			try
			{
				string[] splitted = line.Split(@"\");
				this.Names.Add(splitted[0], splitted[1]);
			}
			catch (Exception ex)
			{
				PageLogger.Log(LoggerType.Error, ex);
			}
		}
	}
	public void FilterSave()
	{
		foreach (var pair in this.DecryptedXml)
		{
			if (pair.Key.Split('.').Length < 4)
			{
				continue;
			}
			try
			{
				string[] splitted = pair.Key.Split(".");
				string id = $"{splitted[0]}.{splitted[1]}";
				this.AllScores.Add(
					JsonConvert.DeserializeObject<ScoreFormat>(this.DecryptedXml[pair.Key])
						.ToInternalFormat(
							this.Difficulties[id][Helper.DifficultStringToIndex(splitted[^1])],
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