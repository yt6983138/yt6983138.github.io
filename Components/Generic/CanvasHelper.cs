﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Numerics;
using System.Reflection;

namespace yt6983138.github.io.Components;

/// <summary>
/// remember to include CanvasHelper.js
/// </summary>
public class CanvasHelper
{
	#region Fields
	private string _direction = "inherit";
	private string _fillStyle = "#000";
	private string _filter = "";
	private string _font = "";
	private string _fontKerning = "auto";
	private string _fontStretch = "normal";
	private string _fontVariantCaps = "normal";
	private float _globalAlpha = 1.0f;
	private string _globalCompositeOperation = "source-over";
	private bool _imageSmoothingEnabled = true;
	private string _imageSmoothingQuality = "low";
	private string _letterSpacing = "0px";
	private string _lineCap = "butt";
	private float _lineDashOffset = 0;
	private string _lineJoin = "miter";
	private float _lineWidth = 1;
	private float _miterLimit = 10;
	private float _shadowBlur = 0;
	private string _shadowColor = "#00000000";
	private float _shadowOffsetX = 0;
	private float _shadowOffsetY = 0;
	private string _strokeStyle = "";
	private string _textAlign = "start";
	private string _textBaseline = "alphabetic";
	private string _textRendering = "auto";
	private string _wordSpacing = "0px";

	[Inject]
	public IJSRuntime runtime { get; set; }
	private string ID = "";
	#endregion

	#region Properties
	// sorry for torture of eyes
	public string Direction { get { return _direction; } set { _direction = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "direction", _direction); } }
	public string FillStyle { get { return _fillStyle; } set { _fillStyle = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "fillStyle", _fillStyle); } }
	public string Filter { get { return _filter; } set { _filter = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "filter", _filter); } }
	public string Font { get { return _font; } set { _font = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "font", _font); } }
	public string FontKerning { get { return _fontKerning; } set { _fontKerning = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "fontKerning", _fontKerning); } }
	public string FontStretch { get { return _fontStretch; } set { _fontStretch = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "fontStretch", _fontStretch); } }
	public string FontVariantCaps { get { return _fontVariantCaps; } set { _fontVariantCaps = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "fontVariantCaps", _fontVariantCaps); } }
	public float GlobalAlpha { get { return _globalAlpha; } set { _globalAlpha = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "globalAlpha", _globalAlpha); } }
	public string GlobalCompositeOperation { get { return _globalCompositeOperation; } set { _globalCompositeOperation = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "globalCompositeOperation", _globalCompositeOperation); } }
	public bool ImageSmoothingEnabled { get { return _imageSmoothingEnabled; } set { _imageSmoothingEnabled = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "imageSmoothingEnabled", _imageSmoothingEnabled); } }
	public string ImageSmoothingQuality { get { return _imageSmoothingQuality; } set { _imageSmoothingQuality = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "imageSmoothingQuality", _imageSmoothingQuality); } }
	public string LetterSpacing { get { return _letterSpacing; } set { _letterSpacing = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "letterSpacing", _letterSpacing); } }
	public string LineCap { get { return _lineCap; } set { _lineCap = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "lineCap", _lineCap); } }
	public float LineDashOffset { get { return _lineDashOffset; } set { _lineDashOffset = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "lineDashOffset", _lineDashOffset); } }
	public string LineJoin { get { return _lineJoin; } set { _lineJoin = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "lineJoin", _lineJoin); } }
	public float LineWidth { get { return _lineWidth; } set { _lineWidth = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "lineWidth", _lineWidth); } }
	public float MiterLimit { get { return _miterLimit; } set { _miterLimit = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "miterLimit", _miterLimit); } }
	public float ShadowBlur { get { return _shadowBlur; } set { _shadowBlur = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "shadowBlur", _shadowBlur); } }
	public string ShadowColor { get { return _shadowColor; } set { _shadowColor = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "shadowColor", _shadowColor); } }
	public float ShadowOffsetX { get { return _shadowOffsetX; } set { _shadowOffsetX = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "shadowOffsetX", _shadowOffsetX); } }
	public float ShadowOffsetY { get { return _shadowOffsetY; } set { _shadowOffsetY = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "shadowOffsetY", _shadowOffsetY); } }
	public string StrokeStyle { get { return _strokeStyle; } set { _strokeStyle = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "strokeStyle", _strokeStyle); } }
	public string TextAlign { get { return _textAlign; } set { _textAlign = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "textAlign", _textAlign); } }
	public string TextBaseline { get { return _textBaseline; } set { _textBaseline = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "textBaseline", _textBaseline); } }
	public string TextRendering { get { return _textRendering; } set { _textRendering = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "textRendering", _textRendering); } }
	public string WordSpacing { get { return _wordSpacing; } set { _wordSpacing = value; runtime!.InvokeVoidAsync("AssignCtxProperty", ID, "wordSpacing", _wordSpacing); } }

	public CanvasHelper Default { get { return new(); } }
	#endregion

	#region Constructor
	public CanvasHelper(string elementIdToCatch, IJSRuntime runtime)
	{
		this.runtime = runtime;
		this.ID = Path.GetRandomFileName();
		runtime!.InvokeVoidAsync("GetCanvasByElementID", elementIdToCatch, this.ID);
	}
	private CanvasHelper()
	{
		return;
	}
	#endregion

	private async Task Run(string funcName, params object?[]? args)
	{
		await runtime!.InvokeVoidAsync("RunCtxFunc", this.ID, funcName, args);
	}
	private async Task<T> Run<T>(string funcName, params object?[]? args)
	{
		return await runtime!.InvokeAsync<T>("RunCtxFunc", this.ID, funcName, args);
	}

	#region Regular methods
	public async void Arc(float x, float y, float radius, float startAngle, float endAngle, bool counterClockwise = false) => await Run("arc", x, y, radius, startAngle, endAngle, counterClockwise);
	public async void ArcTo(float x1, float y1, float x2, float y2, float radius) => await Run("arcTo", x1, y1, x2, y2, radius);
	public async void BeginPath() => await Run("beginPath");
	public async void BezierCurveTo(float cp1x, float cp1y, float cp2x, float cp2y, float x, float y) => await Run("bezierCurveTo", cp1x, cp1y, cp2x, cp2y, x, y);
	public async void ClearRect(float x, float y, float width, float height) => await Run("clearRect", x, y, width, height);
	public async void ClosePath() => await Run("closePath");
	public async void Ellipse(float x, float y, float radiusX, float radiusY, float rotationRadian, float startAngle, float endAngle, bool counterClockwise = false) => await Run("ellipse", x, y, radiusX, radiusY, rotationRadian, startAngle, endAngle, counterClockwise);
	public async void FillRect(float x, float y, float width, float height) => await Run("fillRect", x, y, width, height);
	public async void FillText(string text, float x, float y, float maxWidth = float.MaxValue) => await Run("fillText", text, x, y, maxWidth);
	public async void LineTo(float x, float y) => await Run("lineTo", x, y);
	public async void MoveTo(float x, float y) => await Run("moveTo", x, y);
	public async void QuadraticCurveTo(float cpx, float cpy, float x, float y) => await Run("quadraticCurveTo", cpx, cpy, x, y);
	public async void Rect(float x, float y, float width, float height) => await Run("rect", x, y, width, height);
	public async void Reset()
	{
		var tmp = Default;
		foreach (FieldInfo info in this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
		{
			if (info.Name == "<runtime>k__BackingField" || info.Name == "ID") continue;
			info.SetValue(this, info.GetValue(tmp));
		}
		await Run("reset");
	}
	public async void ResetTransform() => await Run("resetTransform");
	public async void Restore() => await Run("restore");
	public async void Rotate(float rotationRadian) => await Run("rotate", rotationRadian);
	public void RotateEuler(float rotationEuler) => Rotate(rotationEuler * (float)Math.PI / 180);
	public async void RoundRect(float x, float y, float width, float height, string radii) => await Run("roundRect", x, y, width, height, radii);
	public async void Save() => await Run("save");
	public async void Scale(float x, float y) => await Run("scale", x, y);
	public async void SetLineDash(float[] dashes) => await Run("setLineDash", dashes);
	public async void SetTransform(float m11, float m12, float m21, float m22, float m41, float m42) => await Run("setTransform", m11, m12, m21, m22, m41, m42);
	public async void SetTransform(Matrix3x2 matrix) => await Run("setTransform", matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.M31, matrix.M32);
	public async void Stroke() => await Run("stroke");
	public async void StrokeRect(float x, float y, float width, float height) => await Run("strokeRect", x, y, width, height);
	public async void StrokeText(string text, float x, float y, float maxWidth = float.MaxValue) => await Run("strokeText", text, x, y, maxWidth);
	public async void Transform(float m11, float m12, float m21, float m22, float m41, float m42) => await Run("transform", m11, m12, m21, m22, m41, m42);
	public async void Transform(Matrix3x2 matrix) => await Run("transform", matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.M31, matrix.M32);
	public async void Translate(float x, float y) => await Run("translate", x, y);
	#region Not implemented
	public void Fill() => throw new NotImplementedException();
	public void Clip() => throw new NotImplementedException();
	public void DrawFocusIfNeeded() => throw new NotImplementedException();
	public void PutImageData() => throw new NotImplementedException();
	public void Stroke(object path) => throw new NotImplementedException();
	#endregion
	#endregion

	#region Getters
	public async Task<bool> IsContextLost() => await Run<bool>("isContextLost");
	public async Task<(float Width, float Height)> GetCanvasSize()
	{
		return (await runtime!.InvokeAsync<float>("GetCanvasWidth", ID), await runtime!.InvokeAsync<float>("GetCanvasHeight", ID));
	}
	public async Task<(float Width, float Height)> GetImageSize(string id)
	{
		float w = 0;
		float h = 0;
		w = await runtime!.InvokeAsync<float>("GetImageWidth", id);
		h = await runtime!.InvokeAsync<float>("GetImageHeight", id);
		return (w, h);
	}
	#region Not implemented
	public void GetContextAttributes() => throw new NotImplementedException();
	public void GetImageData() => throw new NotImplementedException();
	public void GetLineDash() => throw new NotImplementedException();
	public void GetTransform() => throw new NotImplementedException();
	public bool IsPointInPath() => throw new NotImplementedException();
	public bool IsPointInStroke() => throw new NotImplementedException();
	public void MeasureText() => throw new NotImplementedException();
	#endregion
	#endregion

	#region Create (not implemented at all)
	public void CreateConicGradient(float startAngle, float x, float y) => throw new NotImplementedException();
	public void CreateImageData() => throw new NotImplementedException();
	public void CreateLinearGradient() => throw new NotImplementedException();
	public void CreatePattern() => throw new NotImplementedException();
	public void CreateRadialGradient() => throw new NotImplementedException();
	#endregion

	#region Draw image 
	public async void DrawImage(string ImageId, float sx, float sy, float sWidth, float sHeight, float dx, float dy, float dWidth, float dHeight)
	{
		await runtime!.InvokeVoidAsync("CanvasDrawImg", this.ID, ImageId, sx, sy, sWidth, sHeight, dx, dy, dWidth, dHeight);
	}
	public async void DrawImage(string ImageId, float dx, float dy)
	{
		await runtime!.InvokeVoidAsync("CanvasDrawImg1", this.ID, ImageId, dx, dy);
	}
	public async void DrawImage(string ImageId, float dx, float dy, float dWidth, float dHeight)
	{
		await runtime!.InvokeVoidAsync("CanvasDrawImg2", this.ID, ImageId, dx, dy, dWidth, dHeight);
	}
	public async void AddImageByUrl(string Url, string id)
	{
		await runtime!.InvokeVoidAsync("AddImageByUrl", Url, id);
	}
	public async void RemoveImageById(string id)
	{
		await runtime!.InvokeVoidAsync("RemoveImage", id);
	}
	#endregion
}
