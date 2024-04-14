using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace yt6983138.github.io;
/// <summary>
/// Remember to include EvalHelper.js
/// </summary>
public class EvalHelper
{
	[Inject]
	private IJSRuntime JSRuntime { get; init; }
	public EvalHelper(IJSRuntime jSRuntime)
	{
		this.JSRuntime = jSRuntime;
	}

	public async Task<T> EvalString<T>(string eval, bool useFunction = true, bool useStrict = true)
	{
		return await this.JSRuntime!.InvokeAsync<T>("EvalHelper", eval, useFunction, useStrict);
	}
	public async Task<T> EvalStringWithVariables<T>(string eval, Dictionary<string, object> replace, bool useFunction = true, bool useStrict = true)
	{
		string tmp = eval;
		foreach (KeyValuePair<string, object> pair in replace)
		{
			tmp.Replace(pair.Key, pair.Value.ToString());
		}
		return await this.EvalString<T>(tmp, useFunction, useStrict);
	}
}
