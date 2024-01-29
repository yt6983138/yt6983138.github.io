using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace yt6983138.github.io
{
    /// <summary>
    /// require:
    /// &lt;script src='https://www.gstatic.com/charts/loader.js'>&lt;/script>
    /// &lt;script src="/Assets/Generic/GoogleChartHelper.js">&lt;/script>
    /// </summary>
    public class GoogleChartHelper
    {
        [Inject]
        private IJSRuntime runtime { get; init; }
        public GoogleChartHelper(IJSRuntime runtime)
        {
            this.runtime = runtime;
        }
        public async void Reset()
        {
            await this.runtime!.InvokeVoidAsync("Reset");
        }
        public async void Initalize(string xType, string xTitle, string yType, string yTitle)
        {
            await this.runtime!.InvokeVoidAsync("InitChart", xType, xTitle, yType, yTitle);
        }
        public async void AddRow<T1, T2>(T1 data1, T2 data2)
        {
            await this.runtime!.InvokeVoidAsync("AddRows", data1, data2);
        }
        public async void CreateChart(string elementId)
        {
            await this.runtime!.InvokeVoidAsync("CreateChart", elementId);
        }
        public async void Draw(object? option = null)
        {
            await this.runtime!.InvokeVoidAsync("DrawChart", option);
        }
    }
}
