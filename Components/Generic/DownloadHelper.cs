using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace yt6983138.github.io
{
    /// <summary>
    /// make sure to have &lt;script src="/Assets/Generic/DownloadHelper.js">&lt;/script>
    /// </summary>
    public class DownloadHelper
    {
        [Inject]
        private IJSRuntime JSRuntime { get; init; }
        public DownloadHelper(IJSRuntime runtime)
        {
			this.JSRuntime = runtime;
        }
        public async Task DownloadFromByte(byte[] data, string filename, string contentType)
        {
            await this.JSRuntime!.InvokeVoidAsync("downloadFromData", Convert.ToBase64String(data), filename, contentType);
        }
        public async Task DownloadFromUrl(string url, string filename)
        {
            await this.JSRuntime!.InvokeVoidAsync("downloadFromUrl", url, filename);
        }
    }
}
