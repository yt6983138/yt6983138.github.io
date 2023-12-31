namespace yt6983138.github.io
{
	public class CsvBuilder
	{
		public string[] Header { get; private set; } = Array.Empty<string>();
		public List<string[]> Rows { get; private set; } = new();
		public char Sperator { get; set; } = ',';
		public static string EscapeCSVString(string input, char sperator = ',')
		{
			string first = input.Replace("\"", "\"\"");
			if (first.Contains(sperator) || first.Contains("\n"))
			{
				first = $"\"{first}\"";
			}
			return first;
		}
		public string EscapeCSVString(string input)
		{
			string first = input.Replace("\"", "\"\"");
			if (first.Contains(Sperator) || first.Contains("\n"))
			{
				first = $"\"{first}\"";
			}
			return first;
		}
		public void AddHeader(params string[] headers)
		{
			if (Header.Length != 0)
			{
				throw new Exception("Already have header!");
			}
			Header = new string[headers.Length];
			for (int i = 0; i < Header.Length; i++)
			{
				Header[i] = this.EscapeCSVString(headers[i]);
			}
		}
		public void AddRow(params string[] rows)
		{
			string[] temp = new string[rows.Length];
			for (int i = 0; i < rows.Length; i++)
			{
				temp[i] = this.EscapeCSVString(rows[i]);
			}
			Rows.Add(temp);
		}
		public string Compile()
		{
			string compiled = "";
			for (int i = 0; i < Header.Length; i++)
			{
				compiled += Header[i];
				if (i == Header.Length - 1)
				{
					compiled += "\n";
				} else
				{
					compiled += Sperator;
				}
			}
			for (int i = 0; i < Rows.Count; i++)
			{
				for (int j = 0; j < Rows[i].Length; j++)
				{
					compiled += Rows[i][j];
					if (j == Rows[i].Length - 1)
					{
						compiled += "\n";
					}
					else
					{
						compiled += Sperator;
					}
				}
			}
			return compiled;
		}

	}
}
