namespace yt6983138.github.io;
public enum LoggerType
{
	Info = 0,
	Warning = 1,
	Error = 2,
	Fatal = 3
}
public class Logger
{
	public List<string> AllLogs { get; private set; } = new();
	public string LatestLogMessage { get; private set; } = string.Empty;
	public string LatestLogMessageUnformatted { get; private set; } = string.Empty;
	public string LogFormat { get; set; } = "[{0}] [{1}] {2}\n";
	public string ExceptionFormat { get; set; } = "{0}\nInner: {1}\nStack Trace:\n{2}";
	public Dictionary<LoggerType, string> TypeToMessage { get; set; } = new()
		{
			{ LoggerType.Info, "Info" },
			{ LoggerType.Warning, "Warning" },
			{ LoggerType.Error, "Error" },
			{ LoggerType.Fatal, "Fatal" }
		};
	public void Log(LoggerType type, string message)
	{
		string formatted = string.Format(this.LogFormat, DateTime.Now, this.TypeToMessage[type], message);
		Console.Write(formatted);
		this.AllLogs.Add(formatted);
		this.LatestLogMessageUnformatted = message;
		this.LatestLogMessage = formatted;
	}
	public void Log(LoggerType type, Exception ex)
	{
		string compiled = string.Format(
				this.ExceptionFormat,
				ex.Message,
				ex.InnerException == null ? "Empty" : ex.InnerException.Message,
				ex.StackTrace
				);
		string formatted = string.Format(
			this.LogFormat,
			DateTime.Now,
			this.TypeToMessage[type],
			compiled
			);
		Console.Write(formatted);
		this.AllLogs.Add(formatted);
		this.LatestLogMessageUnformatted = compiled;
		this.LatestLogMessage = formatted;
	}
}
