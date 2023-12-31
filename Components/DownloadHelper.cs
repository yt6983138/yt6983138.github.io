using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace yt6983138.github.io.Components
{
	/// <summary>
	/// make sure to have &lt;script src="/Assets/Generic/DownloadHelper.js">&lt;/script>
	/// </summary>
	public static class DownloadHelper
	{
		public static async Task DownloadFromByte(byte[] data, string filename, string contentType, IJSRuntime JSRuntime)
		{
			await JSRuntime.InvokeVoidAsync("downloadFromData", Convert.ToBase64String(data), filename, contentType);
		}
		public static async Task DownloadFromUrl(string url, string filename, IJSRuntime JSRuntime)
		{
			await JSRuntime.InvokeVoidAsync("downloadFromUrl", url, filename);
		}
	}
}
