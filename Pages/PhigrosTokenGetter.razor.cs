using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PhigrosLibraryCSharp.Cloud.Login;
using PhigrosLibraryCSharp.Cloud.Login.DataStructure;
using System.Net;

namespace yt6983138.github.io.Pages;

public partial class PhigrosTokenGetter
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	[Inject]
	private IJSRuntime JS { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	public string QRCodeAlt { get; set; } = "";
	public string LoginUrl { get; private set; } = "";
	public CompleteQRCodeData? LoginTarget { get; set; }
	public DateTime ExpiresAt { get; set; }
	public int ExpiresInSeconds { get; set; } = 0;
	public string Token { get; set; } = "";
	public bool Generating { get; private set; }
	protected override void OnInitialized()
	{
		LCHelper.GetMD5HashHexString = async value => await this.JS.InvokeAsync<string>("md5", value);
		TapTapHelper.Proxy = async (client, request) =>
		{
			request.RequestUri = new(
				$"https://corsproxy.io/?{request.RequestUri}");
			Console.WriteLine(request.Method);
			return await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
		};
		this.Regenerate();
		this.BackgroundTimer();
		this.BackgroundTask();
		base.OnInitialized();
	}
	public async void Regenerate()
	{
		this.LoginUrl = "Generating...";
		this.ExpiresAt = default;
		this.ExpiresInSeconds = 0;
		this.LoginTarget = null;
		this.Token = "";
		this.QRCodeAlt = "Generating...";
		this.Token = "Waiting for login...";
		this.Generating = true;
		this.StateHasChanged();
		try
		{
			this.LoginTarget = await TapTapHelper.RequestLoginQrCode();
			this.LoginUrl = this.LoginTarget.Url;
			this.ExpiresAt = DateTime.Now + new TimeSpan(0, 0, this.LoginTarget.ExpiresInSeconds);
		}
		catch (Exception ex)
		{
			this.ExpiresAt = default;
			this.LoginUrl = $"Failed to request login url, error: {ex.Message}";
			this.ExpiresInSeconds = 0;
			this.LoginTarget = null;
			this.QRCodeAlt = "Failed to generate!";
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

			TapTapTokenData? result = await TapTapHelper.CheckQRCodeResult(this.LoginTarget);
			if (result is null)
				continue;

			TapTapProfileData profile = await TapTapHelper.GetProfile(result.Data);
			LCCombinedAuthData combined = new(profile.Data, result.Data);

			this.Token = await LCHelper.LoginAndGetToken(combined);

			this.LoginTarget = null;
			this.LoginUrl = "";
			this.ExpiresAt = default;
			this.ExpiresInSeconds = 0;
			this.QRCodeAlt = "Login complete.";
			this.StateHasChanged();
		}
	}
}
