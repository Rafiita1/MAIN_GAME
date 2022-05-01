using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(SharpenRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Sharpen")]
	public sealed class Sharpen : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Intensity of sharpening")]
		public FloatParameter Intensity = new FloatParameter {value = 0.5f};
		internal RenderTexture tempRT0;
	}

	public sealed class SharpenRenderer : PostProcessEffectRenderer<Sharpen>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Sharpen"));
			RenderTexture.ReleaseTemporary(settings.tempRT0);
			sheet.properties.SetFloat("_Intensity", settings.Intensity);
			settings.tempRT0 = RenderTexture.GetTemporary(Screen.width, Screen.height);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}