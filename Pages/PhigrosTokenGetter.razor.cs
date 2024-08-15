using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using PhigrosLibraryCSharp.Cloud.Login.DataStructure;
using System.Net.Http.Json;
using yt6983138.Common;

namespace yt6983138.github.io.Pages;

public partial class PhigrosTokenGetter
{
	#region Endpoints
	public const string GetQrcodeEndpoint = "/api/LoginQrCode/GetNewQrCode";
	public const string CheckQrcodeResultEndpoint = "/api/LoginQrCode/CheckQRCode";
	public const string GetPhigrosTokenEndpoint = "/api/LoginQrCode/GetPhigrosToken";
	#endregion

	#region Settings/clients for backend server
	private string _httpServiceProviderHost = "https://yt6983138.ddns.net";

	public string HttpServiceProviderHost
	{
		get => this._httpServiceProviderHost;
		set
		{
			value = value.TrimEnd('/');
			this._httpServiceProviderHost = value;

			this._httpClient.BaseAddress = new(value);
		}
	}

	private HttpClient _httpClient = new();
	#endregion

	#region Injection
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	[Inject]
	private IJSRuntime JS { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	#endregion

	public string QRCodeAlt { get; set; } = "";
	public string LoginUrl { get; private set; } = "";
	public CompleteQRCodeData? LoginTarget { get; set; }
	public DateTime ExpiresAt { get; set; }
	public int ExpiresInSeconds { get; set; } = 0;
	public string Token { get; set; } = "";
	public bool Generating { get; private set; }
	protected override void OnInitialized()
	{
		base.OnInitialized();

		Task.Delay(50);
		this.Regenerate();
		this.BackgroundTimer();
		this.BackgroundTask();
	}
	public async void Regenerate()
	{
		this.LoginUrl = "Generating...";
		this.ExpiresAt = default;
		this.ExpiresInSeconds = 0;
		this.LoginTarget = null;
		this.Token = "";
		this.QRCodeAlt = "Generating...";
		this.Token = "Generating...";
		this.Generating = true;
		this.StateHasChanged();
		try
		{
			this.LoginTarget = await this.NewQrcode();
			this.LoginUrl = this.LoginTarget.Url;
			this.ExpiresAt = DateTime.Now + new TimeSpan(0, 0, this.LoginTarget.ExpiresInSeconds);
			this.Token = "Waiting for login...";
		}
		catch (Exception ex)
		{
			this.ExpiresAt = default;
			this.LoginUrl = $"Failed to request login url, error: {ex.Message}";
			this.ExpiresInSeconds = 0;
			this.LoginTarget = null;
			this.QRCodeAlt = "Failed to generate!";
			this.Token = "Failed to generate!";
			Console.WriteLine(ex.ToString());
		}
		this.Generating = false;
		this.StateHasChanged();
	}
	public async void BackgroundTimer()
	{
		while (true)
		{
			await Task.Delay(500);
			if (this.ExpiresAt == default)
				continue; // generating

			this.ExpiresInSeconds = (int)(this.ExpiresAt - DateTime.Now).TotalSeconds;
			this.StateHasChanged();
			if (this.ExpiresInSeconds == 0)
				this.Regenerate();
		}
	}
	public async void BackgroundTask()
	{
		while (true)
		{
			await Task.Delay(2500);
			if (this.Generating || this.LoginTarget is null)
				continue; // generating

			TapTapTokenData? result = await this.CheckQrcodeResult(this.LoginTarget);
			if (result is null)
				continue;

			this.Token = await this.GetPhigrosToken(result);

			this.LoginTarget = null;
			this.LoginUrl = "";
			this.ExpiresAt = default;
			this.ExpiresInSeconds = 0;
			this.QRCodeAlt = "Login complete.";
			this.StateHasChanged();
		}
	}
	#region Api methods
	private string GetApiUrl(string endpointUrl)
		=> $"{this.HttpServiceProviderHost}{endpointUrl}";
	public async Task<CompleteQRCodeData> NewQrcode()
	{
		HttpResponseMessage result = await this._httpClient.GetAsync(this.GetApiUrl(GetQrcodeEndpoint));
		return JsonConvert.DeserializeObject<CompleteQRCodeData>(await result.Content.ReadAsStringAsync())
			.EnsureNotNull();
	}
	public async Task<TapTapTokenData?> CheckQrcodeResult(CompleteQRCodeData data)
	{
		HttpResponseMessage result = await this._httpClient.PostAsync(
			this.GetApiUrl(CheckQrcodeResultEndpoint),
			JsonContent.Create(data));
		if (!result.IsSuccessStatusCode)
			return null;
		string content = await result.Content.ReadAsStringAsync();
		TapTapTokenData? ret = JsonConvert.DeserializeObject<TapTapTokenData>(content);
		if (ret is null || ret.Data is null)
			return null;
		return ret;
	}
	public async Task<string> GetPhigrosToken(TapTapTokenData data)
	{
		HttpResponseMessage result = await this._httpClient.PostAsync(
			this.GetApiUrl(GetPhigrosTokenEndpoint),
			JsonContent.Create(data));
		string ret = await result.Content.ReadAsStringAsync();
		result.EnsureSuccessStatusCode();
		return ret;
	}
	#endregion
}
