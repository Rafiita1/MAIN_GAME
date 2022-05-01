using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(ColorCurvesRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/ColorCurves")]
	public sealed class ColorCurves : PostProcessEffectSettings
	{
		[SerializeField]
		internal Texture2D RedEVALUATION;
		[Tooltip("Red channel curve")]
		public SplineParameter Red = new SplineParameter {value = new Spline(new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f)), 0f, false, new Vector2(0f, 1f))};
		[SerializeField]
		internal Texture2D GreenEVALUATION;
		[Tooltip("Green channel curve")]
		public SplineParameter Green = new SplineParameter {value = new Spline(new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f)), 0f, false, new Vector2(0f, 1f))};
		[SerializeField]
		internal Texture2D BlueEVALUATION;
		[Tooltip("Blue channel curve")]
		public SplineParameter Blue = new SplineParameter {value = new Spline(new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f)), 0f, false, new Vector2(0f, 1f))};
		[Tooltip("Image filter")]
		public ColorParameter Tint = new ColorParameter {value = new Color(1f, 1f, 1f, 0f)};

		public void CreateEvaluations(){
			Color pixel = Color.black;
			RedEVALUATION = new Texture2D(128, 1) {wrapMode = TextureWrapMode.Clamp}; 
			for (int i = 0; i < 128; ++i) {
				pixel.a = Red.value.cachedData[i];
				RedEVALUATION.SetPixel(i, 0, pixel);
			}
			RedEVALUATION.Apply();
			GreenEVALUATION = new Texture2D(128, 1) {wrapMode = TextureWrapMode.Clamp}; 
			for (int i = 0; i < 128; ++i) {
				pixel.a = Green.value.cachedData[i];
				GreenEVALUATION.SetPixel(i, 0, pixel);
			}
			GreenEVALUATION.Apply();
			BlueEVALUATION = new Texture2D(128, 1) {wrapMode = TextureWrapMode.Clamp}; 
			for (int i = 0; i < 128; ++i) {
				pixel.a = Blue.value.cachedData[i];
				BlueEVALUATION.SetPixel(i, 0, pixel);
			}
			BlueEVALUATION.Apply();
		}
	}

	public sealed class ColorCurvesRenderer : PostProcessEffectRenderer<ColorCurves>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/ColorCurves"));
			settings.CreateEvaluations();
			sheet.properties.SetTexture("_Red", settings.RedEVALUATION);
			sheet.properties.SetTexture("_Green", settings.GreenEVALUATION);
			sheet.properties.SetTexture("_Blue", settings.BlueEVALUATION);
			sheet.properties.SetColor("_Tint", settings.Tint);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}