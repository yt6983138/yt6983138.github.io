using Microsoft.JSInterop;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;
using System.Net.Http;

namespace yt6983138.github.io.Components.WebPhiCharter;

public static class JSCommands
{
	private static Dictionary<int, Assembly> CachedAssembly = new();
	private static HttpClient _httpClient = new HttpClient();
	private static bool Initialized = false;

	static JSCommands()
	{
#if DEBUG
		_httpClient.BaseAddress = new Uri("https://localhost:7219/");
#else
		_httpClient.BaseAddress = new Uri("https://yt6983138.github.io/");
#endif
		try { EvaluateCs("public class Script { public void Main() {} }"); }
		finally
		{
			Initialized = true;
		}
	}

	[JSInvokable]
	public static void Update(int timeMS)
	{
		while (Initialized == false) { }
		Misc.ChartHolder.Update(timeMS);
	}
	[JSInvokable]
	public static async void EvaluateCs(string code)
	{
		while (Initialized == false) { }
		int hash = code.GetHashCode();
		Assembly assembly;
		if (!CachedAssembly.ContainsKey(hash))
		{
			SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
			string assemblyName = Path.GetRandomFileName();
			Console.WriteLine(typeof(object).Assembly.GetName().Name);
			MetadataReference[] references = new MetadataReference[]
			{
			await GetAssemblyMetadataReference(typeof(object).Assembly),
			await GetAssemblyMetadataReference(typeof(Enumerable).Assembly),
			await GetAssemblyMetadataReference(typeof(Console).Assembly),
			await GetAssemblyMetadataReference("System.Linq"),
			await GetAssemblyMetadataReference("System.Runtime"),
			await GetAssemblyMetadataReference("System.Collections"),
			await GetAssemblyMetadataReference("System.Numerics"),
			await GetAssemblyMetadataReference("System.Numerics.Vectors"),
			await GetAssemblyMetadataReference(typeof(CanvasHelper).Assembly)
			};
			CSharpCompilation compilation = CSharpCompilation.Create(
				assemblyName,
				syntaxTrees: new[] { tree },
				references: references,
				options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
			);
			using (MemoryStream stream = new())
			{
				EmitResult result = compilation.Emit(stream);

				if (!result.Success)
				{
					IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
						diagnostic.IsWarningAsError ||
						diagnostic.Severity == DiagnosticSeverity.Error
					);
					foreach (Diagnostic diagnostic in failures)
					{
						Misc.PageLogger.Log(LoggerType.Error, $"Error while compiling: \n{diagnostic.Id} : {diagnostic.GetMessage()}");
					}
					return;
				}
				else
				{
					stream.Seek(0, SeekOrigin.Begin);
					assembly = Assembly.Load(stream.ToArray());
					CachedAssembly.Add(hash, assembly);
				}
			}
		}
		else
		{
			assembly = CachedAssembly[hash];
		}
		try
		{
			Type type = assembly.GetType("Script")!;
			object obj = Activator.CreateInstance(type!)!;
			type.InvokeMember(
				"Main",
				BindingFlags.Default | BindingFlags.InvokeMethod,
				null,
				obj,
				null
			);
		}
		catch (Exception e)
		{
			Misc.PageLogger.Log(LoggerType.Error, e);
			if (e.InnerException != null)
			{
				Misc.PageLogger.Log(LoggerType.Error, e.InnerException!.StackTrace!);
			}
		}
	}
	private static async Task<MetadataReference> GetAssemblyMetadataReference(Assembly assembly)
	{
		return await GetAssemblyMetadataReference(assembly.GetName().Name!);
	}
	private static async Task<MetadataReference> GetAssemblyMetadataReference(string assmeblyName)
	{
		MetadataReference ret;
		var assemblyUrl = $"/_framework/{assmeblyName}.dll";
		var tmp = await _httpClient.GetAsync(assemblyUrl);
		if (tmp.IsSuccessStatusCode)
		{
			var bytes = await tmp.Content.ReadAsByteArrayAsync();
			ret = MetadataReference.CreateFromImage(bytes);
		}
		else
		{
			throw new Exception("fail");
		}
		return ret;
	}
}
