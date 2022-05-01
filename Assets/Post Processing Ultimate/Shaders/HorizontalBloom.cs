using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(HorizontalBloomRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/HorizontalBloom")]
	public sealed class HorizontalBloom : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Effect intensity")]
		public FloatParameter Intensity = new FloatParameter {value = 0.5f};
		[Range(0f, 1f), Tooltip("Minimum luminosity")]
		public FloatParameter Threshold = new FloatParameter {value = 0.9f};
		[Range(1, 3), Tooltip("Number of blur iterations")]
		public IntParameter Quality = new IntParameter {value = 1};
		internal RenderTexture tempRT0;
		internal RenderTexture tempRT1;
		internal RenderTexture loopRT0;
		internal RenderTexture loopRT1;
	}

	public sealed class HorizontalBloomRenderer : PostProcessEffectRenderer<HorizontalBloom>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/HorizontalBloom"));
			RenderTexture.ReleaseTemporary(settings.tempRT0);
			RenderTexture.ReleaseTemporary(settings.tempRT1);
			RenderTexture.ReleaseTemporary(settings.loopRT0);
			sheet.properties.SetFloat("_Intensity", settings.Intensity);
			sheet.properties.SetFloat("_Threshold", settings.Threshold);
			sheet.properties.SetFloat("_Quality", settings.Quality);
			settings.tempRT0 = RenderTexture.GetTemporary(Screen.width, Screen.height);
			settings.tempRT1 = RenderTexture.GetTemporary(Screen.width, Screen.height);
			context.command.BlitFullscreenTriangle(context.source, settings.tempRT0, sheet, 0);
			settings.loopRT0 = RenderTexture.GetTemporary(Screen.width, Screen.height);
			context.command.BlitFullscreenTriangle(settings.tempRT0, settings.loopRT0);
			for (int i = 0; i < settings.Quality; ++i)
			{
				settings.loopRT1 = RenderTexture.GetTemporary(Screen.width, Screen.height);
				context.command.BlitFullscreenTriangle(settings.loopRT0, settings.loopRT1, sheet, 1);
				RenderTexture.ReleaseTemporary(settings.loopRT0);
				settings.loopRT0 = settings.loopRT1;
			}
			context.command.BlitFullscreenTriangle(settings.loopRT0, settings.tempRT1);
			sheet.properties.SetTexture("_tempRT1", settings.tempRT1);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 2);
		}
	}
}