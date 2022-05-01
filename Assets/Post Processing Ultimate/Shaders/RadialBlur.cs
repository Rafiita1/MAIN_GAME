using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(RadialBlurRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/RadialBlur")]
	public sealed class RadialBlur : PostProcessEffectSettings
	{
		[Range(1, 63), Tooltip("Effect Iterations")]
		public IntParameter Iterations = new IntParameter {value = 4};
		[Range(0f, 1f), Tooltip("Effect Intensity")]
		public FloatParameter Intensity = new FloatParameter {value = 0.5f};
		internal RenderTexture loopRT0;
		internal RenderTexture loopRT1;
	}

	public sealed class RadialBlurRenderer : PostProcessEffectRenderer<RadialBlur>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/RadialBlur"));
			RenderTexture.ReleaseTemporary(settings.loopRT0);
			sheet.properties.SetFloat("_Iterations", settings.Iterations);
			sheet.properties.SetFloat("_Intensity", settings.Intensity);
			settings.loopRT0 = RenderTexture.GetTemporary(Screen.width, Screen.height);
			context.command.BlitFullscreenTriangle(context.source, settings.loopRT0);
			for (int i = 0; i < settings.Iterations; ++i)
			{
				settings.loopRT1 = RenderTexture.GetTemporary(Screen.width, Screen.height);
				context.command.BlitFullscreenTriangle(settings.loopRT0, settings.loopRT1, sheet, 0);
				RenderTexture.ReleaseTemporary(settings.loopRT0);
				settings.loopRT0 = settings.loopRT1;
			}
			context.command.BlitFullscreenTriangle(settings.loopRT0, context.destination);
		}
	}
}