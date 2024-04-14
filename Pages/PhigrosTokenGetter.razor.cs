﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PhigrosLibraryCSharp.Cloud.Login;
using PhigrosLibraryCSharp.Cloud.Login.DataStructure;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace yt6983138.github.io.Pages;

public partial class PhigrosTokenGetter
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	[Inject]
	private IJSRuntime JS { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	public string QRCodeBlob { get; set; } = "";
	public string QRCodeAlt { get; set; } = "";
	public string LoginUrl { get; private set; } = "";
	public CompleteQRCodeData? LoginTarget { get; set; }
	public DateTime ExpiresAt { get; set; }
	public int ExpiresInSeconds { get; set; } = 0;
	public bool UseChinaEndpoint { get; set; } = false;
	public string Token { get; set; } = "";
	public bool Generating { get; private set; }
	protected override void OnInitialized()
	{
		LCHelper.GetMD5HashHexString = async value => await this.JS.InvokeAsync<string>("md5", value);
		this.Regenerate();
		base.OnInitialized();
	}
	public async void Regenerate()
	{
		this.LoginUrl = "Generating...";
		this.QRCodeBlob = "";
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
			this.LoginTarget = await TapTapHelper.RequestLoginQrCode(useChinaEndpoint: this.UseChinaEndpoint);
			this.LoginUrl = this.LoginTarget.Url;
			this.ExpiresAt = DateTime.Now + new TimeSpan(0, 0, this.LoginTarget.ExpiresInSeconds);

			using MemoryStream ms = new();
			QRCodeGenerator qrCodeGenerate = new();
			QRCodeData qrCodeData = qrCodeGenerate.CreateQrCode(this.LoginUrl, QRCodeGenerator.ECCLevel.Q);
			QRCode qrCode = new(qrCodeData);
			using Bitmap qrBitMap = qrCode.GetGraphic(20);
			qrBitMap.Save(ms, ImageFormat.Png);
			string base64 = Convert.ToBase64String(ms.ToArray());
			this.QRCodeBlob = string.Format("data:image/png;base64,{0}", base64);
			this.QRCodeAlt = "Generated!";
		}
		catch (Exception ex)
		{
			this.QRCodeBlob = "";
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
			if (this.ExpiresInSeconds < 0)
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

			TapTapTokenData? result = await TapTapHelper.CheckQRCodeResult(this.LoginTarget, this.UseChinaEndpoint);
			if (result is null)
				continue;

			TapTapProfileData profile = await TapTapHelper.GetProfile(result.Data, this.UseChinaEndpoint);
			LCCombinedAuthData combined = new(profile.Data, result.Data);

			this.Token = await LCHelper.LoginAndGetToken(combined);

			this.LoginTarget = null;
			this.LoginUrl = "";
			this.QRCodeBlob = "";
			this.ExpiresAt = default;
			this.ExpiresInSeconds = 0;
			this.QRCodeAlt = "Login complete.";
			this.StateHasChanged();
		}
	}
}
