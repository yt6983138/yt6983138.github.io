using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
	public string Direction { get { return this._direction; } set { this._direction = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "direction", this._direction); } }
	public string FillStyle { get { return this._fillStyle; } set { this._fillStyle = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "fillStyle", this._fillStyle); } }
	public string Filter { get { return this._filter; } set { this._filter = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "filter", this._filter); } }
	public string Font { get { return this._font; } set { this._font = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "font", this._font); } }
	public string FontKerning { get { return this._fontKerning; } set { this._fontKerning = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "fontKerning", this._fontKerning); } }
	public string FontStretch { get { return this._fontStretch; } set { this._fontStretch = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "fontStretch", this._fontStretch); } }
	public string FontVariantCaps { get { return this._fontVariantCaps; } set { this._fontVariantCaps = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "fontVariantCaps", this._fontVariantCaps); } }
	public float GlobalAlpha { get { return this._globalAlpha; } set { this._globalAlpha = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "globalAlpha", this._globalAlpha); } }
	public string GlobalCompositeOperation { get { return this._globalCompositeOperation; } set { this._globalCompositeOperation = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "globalCompositeOperation", this._globalCompositeOperation); } }
	public bool ImageSmoothingEnabled { get { return this._imageSmoothingEnabled; } set { this._imageSmoothingEnabled = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "imageSmoothingEnabled", this._imageSmoothingEnabled); } }
	public string ImageSmoothingQuality { get { return this._imageSmoothingQuality; } set { this._imageSmoothingQuality = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "imageSmoothingQuality", this._imageSmoothingQuality); } }
	public string LetterSpacing { get { return this._letterSpacing; } set { this._letterSpacing = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "letterSpacing", this._letterSpacing); } }
	public string LineCap { get { return this._lineCap; } set { this._lineCap = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "lineCap", this._lineCap); } }
	public float LineDashOffset { get { return this._lineDashOffset; } set { this._lineDashOffset = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "lineDashOffset", this._lineDashOffset); } }
	public string LineJoin { get { return this._lineJoin; } set { this._lineJoin = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "lineJoin", this._lineJoin); } }
	public float LineWidth { get { return this._lineWidth; } set { this._lineWidth = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "lineWidth", this._lineWidth); } }
	public float MiterLimit { get { return this._miterLimit; } set { this._miterLimit = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "miterLimit", this._miterLimit); } }
	public float ShadowBlur { get { return this._shadowBlur; } set { this._shadowBlur = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "shadowBlur", this._shadowBlur); } }
	public string ShadowColor { get { return this._shadowColor; } set { this._shadowColor = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "shadowColor", this._shadowColor); } }
	public float ShadowOffsetX { get { return this._shadowOffsetX; } set { this._shadowOffsetX = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "shadowOffsetX", this._shadowOffsetX); } }
	public float ShadowOffsetY { get { return this._shadowOffsetY; } set { this._shadowOffsetY = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "shadowOffsetY", this._shadowOffsetY); } }
	public string StrokeStyle { get { return this._strokeStyle; } set { this._strokeStyle = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "strokeStyle", this._strokeStyle); } }
	public string TextAlign { get { return this._textAlign; } set { this._textAlign = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "textAlign", this._textAlign); } }
	public string TextBaseline { get { return this._textBaseline; } set { this._textBaseline = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "textBaseline", this._textBaseline); } }
	public string TextRendering { get { return this._textRendering; } set { this._textRendering = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "textRendering", this._textRendering); } }
	public string WordSpacing { get { return this._wordSpacing; } set { this._wordSpacing = value; this.runtime!.InvokeVoidAsync("AssignCtxProperty", this.ID, "wordSpacing", this._wordSpacing); } }

	public CanvasHelper Default { get { return new(); } }
	#endregion

	#region Constructor
	public CanvasHelper(string elementIdToCatch, IJSRuntime runtime)
	{
		this.runtime = runtime;
		this.ID = Path.GetRandomFileName();
		runtime!.InvokeVoidAsync("GetCanvasByElementID", elementIdToCatch, this.ID);
	}
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private CanvasHelper()
	{
		return;
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	#endregion

	private async Task Run(string funcName, params object?[]? args)
	{
		await this.runtime!.InvokeVoidAsync("RunCtxFunc", this.ID, funcName, args);
	}
	private async Task<T> Run<T>(string funcName, params object?[]? args)
	{
		return await this.runtime!.InvokeAsync<T>("RunCtxFunc", this.ID, funcName, args);
	}

	#region Regular methods
	public async void Arc(float x, float y, float radius, float startAngle, float endAngle, bool counterClockwise = false) => await this.Run("arc", x, y, radius, startAngle, endAngle, counterClockwise);
	public async void ArcTo(float x1, float y1, float x2, float y2, float radius) => await this.Run("arcTo", x1, y1, x2, y2, radius);
	public async void BeginPath() => await this.Run("beginPath");
	public async void BezierCurveTo(float cp1x, float cp1y, float cp2x, float cp2y, float x, float y) => await this.Run("bezierCurveTo", cp1x, cp1y, cp2x, cp2y, x, y);
	public async void ClearRect(float x, float y, float width, float height) => await this.Run("clearRect", x, y, width, height);
	public async void ClosePath() => await this.Run("closePath");
	public async void Ellipse(float x, float y, float radiusX, float radiusY, float rotationRadian, float startAngle, float endAngle, bool counterClockwise = false) => await this.Run("ellipse", x, y, radiusX, radiusY, rotationRadian, startAngle, endAngle, counterClockwise);
	public async void FillRect(float x, float y, float width, float height) => await this.Run("fillRect", x, y, width, height);
	public async void FillText(string text, float x, float y, float maxWidth = float.MaxValue) => await this.Run("fillText", text, x, y, maxWidth);
	public async void LineTo(float x, float y) => await this.Run("lineTo", x, y);
	public async void MoveTo(float x, float y) => await this.Run("moveTo", x, y);
	public async void QuadraticCurveTo(float cpx, float cpy, float x, float y) => await this.Run("quadraticCurveTo", cpx, cpy, x, y);
	public async void Rect(float x, float y, float width, float height) => await this.Run("rect", x, y, width, height);
	public async void Reset()
	{
		CanvasHelper tmp = this.Default;
		foreach (FieldInfo info in this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
		{
			if (info.Name == "<runtime>k__BackingField" || info.Name == "ID") continue;
			info.SetValue(this, info.GetValue(tmp));
		}
		await this.Run("reset");
	}
	public async void ResetTransform() => await this.Run("resetTransform");
	public async void Restore() => await this.Run("restore");
	public async void Rotate(float rotationRadian) => await this.Run("rotate", rotationRadian);
	public void RotateEuler(float rotationEuler) => this.Rotate(rotationEuler * (float)Math.PI / 180);
	public async void RoundRect(float x, float y, float width, float height, string radii) => await this.Run("roundRect", x, y, width, height, radii);
	public async void Save() => await this.Run("save");
	public async void Scale(float x, float y) => await this.Run("scale", x, y);
	public async void SetLineDash(float[] dashes) => await this.Run("setLineDash", dashes);
	public async void SetTransform(float m11, float m12, float m21, float m22, float m41, float m42) => await this.Run("setTransform", m11, m12, m21, m22, m41, m42);
	public async void SetTransform(Matrix3x2 matrix) => await this.Run("setTransform", matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.M31, matrix.M32);
	public async void Stroke() => await this.Run("stroke");
	public async void StrokeRect(float x, float y, float width, float height) => await this.Run("strokeRect", x, y, width, height);
	public async void StrokeText(string text, float x, float y, float maxWidth = float.MaxValue) => await this.Run("strokeText", text, x, y, maxWidth);
	public async void Transform(float m11, float m12, float m21, float m22, float m41, float m42) => await this.Run("transform", m11, m12, m21, m22, m41, m42);
	public async void Transform(Matrix3x2 matrix) => await this.Run("transform", matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.M31, matrix.M32);
	public async void Translate(float x, float y) => await this.Run("translate", x, y);
	#region Not implemented
	public void Fill() => throw new NotImplementedException();
	public void Clip() => throw new NotImplementedException();
	public void DrawFocusIfNeeded() => throw new NotImplementedException();
	public void PutImageData() => throw new NotImplementedException();
	public void Stroke(object path) => throw new NotImplementedException();
	#endregion
	#endregion

	#region Getters
	public async Task<bool> IsContextLost() => await this.Run<bool>("isContextLost");
	public async Task<(float Width, float Height)> GetCanvasSize()
	{
		return (await this.runtime!.InvokeAsync<float>("GetCanvasWidth", this.ID), await this.runtime!.InvokeAsync<float>("GetCanvasHeight", this.ID));
	}
	public async Task<(float Width, float Height)> GetImageSize(string id)
	{
		float w = 0;
		float h = 0;
		w = await this.runtime!.InvokeAsync<float>("GetImageWidth", id);
		h = await this.runtime!.InvokeAsync<float>("GetImageHeight", id);
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
		await this.runtime!.InvokeVoidAsync("CanvasDrawImg", this.ID, ImageId, sx, sy, sWidth, sHeight, dx, dy, dWidth, dHeight);
	}
	public async void DrawImage(string ImageId, float dx, float dy)
	{
		await this.runtime!.InvokeVoidAsync("CanvasDrawImg1", this.ID, ImageId, dx, dy);
	}
	public async void DrawImage(string ImageId, float dx, float dy, float dWidth, float dHeight)
	{
		await this.runtime!.InvokeVoidAsync("CanvasDrawImg2", this.ID, ImageId, dx, dy, dWidth, dHeight);
	}
	public async void AddImageByUrl(string Url, string id)
	{
		await this.runtime!.InvokeVoidAsync("AddImageByUrl", Url, id);
	}
	public async void RemoveImageById(string id)
	{
		await this.runtime!.InvokeVoidAsync("RemoveImage", id);
	}
	#endregion
}
