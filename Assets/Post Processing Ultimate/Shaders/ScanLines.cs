using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(ScanLinesRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/ScanLines")]
	public sealed class ScanLines : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Effect intensity")]
		public FloatParameter Intensity = new FloatParameter {value = 0.5f};
		[Range(0f, 1f), Tooltip("Line width")]
		public FloatParameter Width = new FloatParameter {value = 0.5f};
		[Range(0f, 1f), Tooltip("Gap between lines")]
		public FloatParameter Gap = new FloatParameter {value = 0.5f};
		internal RenderTexture tempRT0;
	}

	public sealed class ScanLinesRenderer : PostProcessEffectRenderer<ScanLines>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/ScanLines"));
			RenderTexture.ReleaseTemporary(settings.tempRT0);
			sheet.properties.SetFloat("_Intensity", settings.Intensity);
			sheet.properties.SetFloat("_Width", settings.Width);
			sheet.properties.SetFloat("_Gap", settings.Gap);
			settings.tempRT0 = RenderTexture.GetTemporary(Screen.width, Screen.height);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}