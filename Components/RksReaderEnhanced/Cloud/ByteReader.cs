﻿using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;

namespace yt6983138.github.io.RksReaderEnhanced;

//9D 01 16 47 6C 61 63 69 61 78 69 6F 6E 2E 53 75 6E 73 65 74 52 61 79 2E 30 1A 07 07 40 42 0F 00 00 00 C8 42 6B 39 0F 00 64 7F C7 42 40 42 0F 00 00 00 C8 42 0F 43 72 65 64 69 74 73 2E 46 72 75 6D 73 2E 30 12 0C 00 B7 13 0E 00 D3 F8 BE 42 67 D5 0D 00 76 12 C1 42 0F E5 85 89 2E E5 A7 9C E7
//Glaciaxion.SunsetRay.0@B���ÈBk9�dÇB@B���ÈBCredits.Frums.0�·�Óø¾Bg

// (9D 01)		| (16)				(47 ... 30)		 (1A)							(07)	(07)		(40 ... 42)														|
// header		| id string length	id string		 record Offset (need to +1)		is fc	unknown		record start (structure: (int score then float acc) -> repeat)	| repeat (in pipe)
// read header	| read string bytes					|read record																								| read string... 

[StructLayout(LayoutKind.Sequential)]
public struct PartialGameRecord
{
	//[FieldOffset(0)]
	public int Score;
	//[FieldOffset(4)]
	public float Acc;
}
public struct MoreInfoPartialGameRecord
{
	public int Score;
	public float Acc;
	public bool IsFc;
	public int LevelType;

	public MoreInfoPartialGameRecord(PartialGameRecord data, bool isfc, int levelType)
	{
		Score = data.Score;
		Acc = data.Acc;
		IsFc = isfc;
		LevelType = levelType;
	}
}
public class ByteReader // fuck my brain is going to explode if i keep working on this shit
{
	public byte[] Data { get; set; }
	public int Offset { get; private set; }
	public int RecordRead { get; private set; } = 0;
	public static IReadOnlyDictionary<int, string> IntLevelToStringLevel { get; } = new Dictionary<int, string>()
	{
		{ 0, "EZ" },
		{ 1, "HD" },
		{ 2, "IN" },
		{ 3, "AT" }
	};

	public ByteReader(byte[] data, int offset = 0)
	{
		this.Offset = offset;
		this.Data = data;
	}
	public bool ReadIsFc() // i have no idea why is it like this
	{
		Offset++;
		return (Data[Offset - 1] & 1 << RecordRead) == 0 ? false : true;
	}
	public bool ReadBool(int num, int index)
	{
		return (num & 1 << index) == 0 ? false : true;
	}
	public void ReadHeader(int size)
	{
		Offset += size;
	}
	public byte[] ReadStringBytes()
	{
		RecordRead++;
		var data = Data[(Offset + 1)..(Offset + Data[Offset] + 1)];
		Offset += Data[Offset] + 1;
		return data;
	}
	public List<MoreInfoPartialGameRecord> ReadRecord()
	{
		List<MoreInfoPartialGameRecord> scores = new();
		int readLen = Data[Offset - 3] - 2;
		int endOffset = readLen + Offset;
		byte exists = Data[Offset - 2];
		byte fc = Data[Offset - 1];

		// Console.WriteLine((endOffset - Offset).ToString("X4"));
		// Console.WriteLine(Offset.ToString("X4"));
		// Console.WriteLine(endOffset.ToString("X4"));
		for (byte i = 0; i < 4; i++)
		{
			// Console.WriteLine(BitConverter.ToString(Data[Offset..(Offset + 8)]));
			if (Offset == endOffset) break;
			if (!ReadBool(exists, i) || Offset + 8 >= Data.Length)
			{
				// Offset += 8;
				continue;
			}
			var record = SerialHelper.ByteToStruct<PartialGameRecord>(Data[Offset..(Offset + 8)]);
			if (record.Acc > 100 || record.Acc < 0) Console.WriteLine(BitConverter.ToString(Data[Offset..(Offset + 8)]).Replace('-', ' '));
			scores.Add(
				new MoreInfoPartialGameRecord(
					record,
					ReadBool(fc, i),
					i
				)
			);
			Offset += 8;
		}
		Offset = endOffset;
		// Offset++;
		return scores;
	}
	public void Jump(int offset)
	{
		Offset += offset;
	}
	public List<InternalScoreFormat> ReadAll(in IReadOnlyDictionary<string, float[]> difficulties)
	{
		this.ReadHeader(2);
		List<InternalScoreFormat> scores = new();
		while (this.Offset < this.Data.Length)
		{
			string id = Encoding.UTF8.GetString(this.ReadStringBytes())[..^2];
			this.Jump(3);

			foreach (var item in this.ReadRecord())
			{
				scores.Add(new InternalScoreFormat(item, id, difficulties[id][item.LevelType], IntLevelToStringLevel));
			}
		}
		return scores;
	}
}
