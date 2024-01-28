﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace yt6983138.github.io.RksReaderEnhanced;
public class SaveHelper // generated by ai
{
	#region Constants
	private readonly static byte[] sBox = new byte[256]
	{
		82, 9, 106, 213, 48, 54, 165, 56, 191, 64, 163, 158, 129, 243, 215, 251,
		124, 227, 57, 130, 155, 47, 255, 135, 52, 142, 67, 68, 196, 222, 233, 203,
		84, 123, 148, 50, 166, 194, 35, 61, 238, 76, 149, 11, 66, 250, 195, 78, 8,
		46, 161, 102, 40, 217, 36, 178, 118, 91, 162, 73, 109, 139, 209, 37, 114,
		248, 246, 100, 134, 104, 152, 22, 212, 164, 92, 204, 93, 101, 182, 146,
		108, 112, 72, 80, 253, 237, 185, 218, 94, 21, 70, 87, 167, 141, 157, 132,
		144, 216, 171, 0, 140, 188, 211, 10, 247, 228, 88, 5, 184, 179, 69, 6,
		208, 44, 30, 143, 202, 63, 15, 2, 193, 175, 189, 3, 1, 19, 138, 107, 58,
		145, 17, 65, 79, 103, 220, 234, 151, 242, 207, 206, 240, 180, 230, 115,
		150, 172, 116, 34, 231, 173, 53, 133, 226, 249, 55, 232, 28, 117, 223,
		110, 71, 241, 26, 113, 29, 41, 197, 137, 111, 183, 98, 14, 170, 24, 190,
		27, 252, 86, 62, 75, 198, 210, 121, 32, 154, 219, 192, 254, 120, 205, 90,
		244, 31, 221, 168, 51, 136, 7, 199, 49, 177, 18, 16, 89, 39, 128, 236, 95,
		96, 81, 127, 169, 25, 181, 74, 13, 45, 229, 122, 159, 147, 201, 156, 239,
		160, 224, 59, 77, 174, 42, 245, 176, 200, 235, 187, 60, 131, 83, 153, 97,
		23, 43, 4, 126, 186, 119, 214, 38, 225, 105, 20, 99, 85, 33, 12, 125
	};
	private readonly static byte[] invSBox = new byte[240]
	{
		98, 127, 241, 148, 33, 133, 224, 17, 200, 21, 232, 30, 99, 155, 154, 0, 0,
		28, 118, 107, 130, 108, 41, 189, 150, 87, 133, 137, 241, 154, 111, 214,
		219, 215, 7, 53, 250, 82, 231, 36, 50, 71, 15, 58, 81, 220, 149, 58, 209,
		154, 92, 235, 83, 246, 117, 86, 197, 161, 240, 223, 52, 59, 159, 9, 59,
		12, 6, 45, 193, 94, 225, 9, 243, 25, 238, 51, 162, 197, 123, 9, 235, 60,
		125, 234, 184, 202, 8, 188, 125, 107, 248, 99, 73, 80, 103, 106, 108, 137,
		4, 22, 173, 215, 229, 31, 94, 206, 11, 44, 252, 11, 112, 37, 91, 23, 44,
		213, 227, 221, 36, 105, 158, 182, 220, 10, 215, 230, 187, 96, 234, 99,
		212, 24, 71, 180, 49, 7, 25, 122, 58, 43, 229, 113, 74, 14, 130, 180, 250,
		126, 97, 105, 222, 23, 255, 223, 2, 29, 40, 57, 185, 125, 232, 53, 43, 44,
		175, 129, 26, 43, 182, 251, 32, 0, 83, 138, 106, 14, 111, 202, 248, 213,
		14, 163, 38, 194, 241, 124, 36, 223, 217, 69, 157, 162, 166, 107, 17, 25,
		9, 234, 11, 50, 191, 17, 43, 50, 236, 155, 65, 60, 161, 222, 123, 62, 175,
		125, 93, 252, 94, 1, 121, 35, 135, 68, 228, 129, 253, 2, 29, 14, 244, 232,
		22, 60, 75, 249, 61, 14, 167, 98, 124, 50
	};
	private readonly static byte[] initialVector = new byte[16] { 190, 86, 22, 127, 131, 218, 59, 239, 239, 248, 24, 97, 165, 197, 243, 205 };
	private readonly static byte[] rcon = new byte[256]
	{
		0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40,
		42, 44, 46, 48, 50, 52, 54, 56, 58, 60, 62, 64, 66, 68, 70, 72, 74, 76, 78,
		80, 82, 84, 86, 88, 90, 92, 94, 96, 98, 100, 102, 104, 106, 108, 110, 112,
		114, 116, 118, 120, 122, 124, 126, 128, 130, 132, 134, 136, 138, 140, 142,
		144, 146, 148, 150, 152, 154, 156, 158, 160, 162, 164, 166, 168, 170, 172,
		174, 176, 178, 180, 182, 184, 186, 188, 190, 192, 194, 196, 198, 200, 202,
		204, 206, 208, 210, 212, 214, 216, 218, 220, 222, 224, 226, 228, 230, 232,
		234, 236, 238, 240, 242, 244, 246, 248, 250, 252, 254, 27, 25, 31, 29, 19, 17,
		23, 21, 11, 9, 15, 13, 3, 1, 7, 5, 59, 57, 63, 61, 51, 49, 55, 53, 43, 41, 47,
		45, 35, 33, 39, 37, 91, 89, 95, 93, 83, 81, 87, 85, 75, 73, 79, 77, 67, 65,
		71, 69, 123, 121, 127, 125, 115, 113, 119, 117, 107, 105, 111, 109, 99, 97,
		103, 101, 155, 153, 159, 157, 147, 145, 151, 149, 139, 137, 143, 141, 131,
		129, 135, 133, 187, 185, 191, 189, 179, 177, 183, 181, 171, 169, 175, 173,
		163, 161, 167, 165, 219, 217, 223, 221, 211, 209, 215, 213, 203, 201, 207,
		205, 195, 193, 199, 197, 251, 249, 255, 253, 243, 241, 247, 245, 235, 233,
		239, 237, 227, 225, 231, 229
	};
	private readonly static byte[] tempArray = new byte[16] { 0, 13, 10, 7, 4, 1, 14, 11, 8, 5, 2, 15, 12, 9, 6, 3 };

	#region Cloud
	private const string CloudAESKey = @"6Jaa0qVAJZuXkZCLiOa/Ax5tIZVu+taKUN1V1nqwkks=";
	private const string CloudAESIV = @"Kk/wisgNYwcAV8WVGMgyUw==";
	private const string CloudMeAddress = @"https://rak3ffdi.cloud.tds1.tapapis.cn/1.1/users/me";
	private const string CloudGameSaveAddress = @"https://rak3ffdi.cloud.tds1.tapapis.cn/1.1/classes/_GameSave";
	private readonly JsonSerializerSettings SerializerSettings = new()
	{
		Error = (_, args) =>
		{
			args.ErrorContext.Handled = true;
		}
	};

	[Inject]
	private IJSRuntime Runtime { get; init; }
	private string? SessionToken { get; set; } = null;
	private HttpClient Client { get; set; } = new();
	#endregion
	#endregion

	#region Save decrypt (from local save)
	public static string DecryptSaveStringNew(string base64EncryptedString)
	{
		byte[] state = QuickCopy(initialVector); // readonly != immutable

		byte[] cipherTextCopy = Convert.FromBase64String(base64EncryptedString);
		for (int e = 0; e < cipherTextCopy.Length; e += 16)
		{
			byte[] block = cipherTextCopy[e..(e + 16)];
			addRoundKey(ref block, invSBox[224..240]);
			shiftRows(ref block);
			subBytes(ref block);
			for (int i = 208; i >= 16; i -= 16)
			{
				addRoundKey(ref block, invSBox[i..(i + 16)]);
				mixColumns(ref block);
				shiftRows(ref block);
				subBytes(ref block);
			}
			addRoundKey(ref block, invSBox[0..16]);
			for (int j = 0; j < 16; j++) block[j] ^= state[j];
			for (int t = 0; t < 16; t++) state[t] = cipherTextCopy[e + t];
			for (int n = 0; n < 16; n++) cipherTextCopy[e + n] = block[n];
			for (int m = e; m < e + 16; m++)
			{
				cipherTextCopy[m] = block[m - e];
			}
		}
		byte[] computed = cipherTextCopy[0..^cipherTextCopy[^1]];
		return Encoding.UTF8.GetString(computed);
		// return Encoding.ASCII.GetString(computed);
	}
	private static void subBytes(ref byte[] state)
	{
		for (byte t = 0; t < 16; t++)
		{
			state[t] = sBox[state[t]];
		}
	}
	private static void shiftRows(ref byte[] state)
	{
		byte[] tempState = QuickCopy(state);
		for (byte i = 0; i < 16; i++)
		{
			state[i] = tempState[tempArray[i]];
		}
	}
	private static void addRoundKey(ref byte[] state, in byte[] roundKey)
	{
		for (byte n = 0; n < 16; n++)
		{
			state[n] ^= roundKey[n];
		}
	}
	private static void mixColumns(ref byte[] state)
	{
		for (byte n = 0; n < 16; n += 4)
		{
			byte i0 = state[n + 0];
			byte i1 = state[n + 1];
			byte i2 = state[n + 2];
			byte i3 = state[n + 3];
			int sBoxColumnSum = i0 ^ i1 ^ i2 ^ i3;
			byte rconColumnSum = rcon[sBoxColumnSum];
			int doubleRconColumnSumI0I2 = rcon[rcon[rconColumnSum ^ i0 ^ i2]] ^ sBoxColumnSum;
			int doubleRconColumnSumI1I3 = rcon[rcon[rconColumnSum ^ i1 ^ i3]] ^ sBoxColumnSum;
			state[n + 0] ^= (byte)(doubleRconColumnSumI0I2 ^ rcon[i0 ^ i1]);
			state[n + 1] ^= (byte)(doubleRconColumnSumI1I3 ^ rcon[i1 ^ i2]);
			state[n + 2] ^= (byte)(doubleRconColumnSumI0I2 ^ rcon[i2 ^ i3]);
			state[n + 3] ^= (byte)(doubleRconColumnSumI1I3 ^ rcon[i3 ^ i0]);
		}
	}
	private static T[] QuickCopy<T>(T[] array)
	{
		T[] values = new T[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			values[i] = array[i];
		}
		return values;
	}
	#endregion

	#region Cloud save
	public SaveHelper(IJSRuntime runtime)
	{
		Runtime = runtime;
	}
	public void InitializeCloudHelper(string sessionToken)
	{
		SessionToken = sessionToken;
		Client = new();
		Client.DefaultRequestHeaders.Add("X-LC-Id", "rAK3FfdieFob2Nn8Am");
		Client.DefaultRequestHeaders.Add("X-LC-Key", "Qr9AEqtuoSVS3zeD6iVbM4ZC0AtkJcQ89tywVyi0");
		Client.DefaultRequestHeaders.Add("User-Agent", "LeanCloud-CSharp-SDK/1.0.3");
		Client.DefaultRequestHeaders.Add("Accept", "application/json");
		Client.DefaultRequestHeaders.Add("X-LC-Session", sessionToken);

		try
		{
			_ = GetUserInfo();
		}
		catch
		{
			throw new Exception("Invalid token.");
		}
	}
	public async Task<RawSaveContainer> GetRawSaveFromCloud()
	{
		if (SessionToken == null) throw new Exception("Session token cannot be null.");
		HttpResponseMessage response = await Client.GetAsync(CloudGameSaveAddress);
		string content = await response.Content.ReadAsStringAsync();
		var container = JsonConvert.DeserializeObject<RawSaveContainer>(content, SerializerSettings);
		return container;
	}
	public async Task<List<GameSave>> GetGameSaves(IReadOnlyDictionary<string, float[]> difficulties)
	{
		List<SimplifiedSave> raw = (await GetRawSaveFromCloud()).GetParsedSaves();
		Console.WriteLine(raw.Count);

		byte[] iv = Convert.FromBase64String(CloudAESIV);
		byte[] key = Convert.FromBase64String(CloudAESKey);

		List<GameSave> saves = new();
		foreach (SimplifiedSave save in raw)
		{
			byte[] rawData;
			try
			{
				rawData = await Client.GetByteArrayAsync(save.GameSave.Url); // note raw data is zip
			} catch {Console.WriteLine("failed"); continue; }
			using (ZipFile zipFile = new ZipFile(new MemoryStream(rawData)))
			{
				ZipEntry recordFile = zipFile.GetEntry("gameRecord");
				if (recordFile == null) continue;

				byte[] decompressed = new byte[recordFile.Size];
				zipFile.GetInputStream(recordFile).Read(decompressed, 0, decompressed.Length);
				decompressed = decompressed[1..]; // for some reason i need to trim the first byte

				byte[] decrypted = await Runtime!.InvokeAsync<byte[]>("AesDecrypt", decompressed, key, iv); // c# aes not supported (why microsoft)
				// Console.WriteLine(BitConverter.ToString(decompressed).Replace('-', ' '));
				// Console.WriteLine(BitConverter.ToString(decrypted).Replace('-', ' '));
				ByteReader byteReader = new(decrypted);
				GameSave gameSave = new()
				{
					CreationDate = save.CreationDate,
					ModificationTime = save.ModificationTime,
					Records = byteReader.ReadAll(difficulties),
					Summary = save.Summary
				};
				saves.Add(gameSave);
			}
		}

		return saves;

	}
	public async Task<byte[]> GetRawAddress(string address)
	{
		if (SessionToken == null) throw new Exception("Session token cannot be null.");
		HttpResponseMessage response = await Client.GetAsync(address);
		var content = await response.Content.ReadAsByteArrayAsync();

		return content;
	}
	public async Task<UserInfo> GetUserInfo()
	{
		if (SessionToken == null) throw new Exception("Session token cannot be null.");
		HttpResponseMessage response = await Client.GetAsync(CloudMeAddress);
		string content = await response.Content.ReadAsStringAsync();
		return JsonConvert.DeserializeObject<UserInfo>(content, SerializerSettings);
	}
	#endregion
}
