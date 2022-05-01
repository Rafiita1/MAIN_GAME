using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(BlurRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Blur")]
	public sealed class Blur : PostProcessEffectSettings
	{
		[Range(1, 16), Tooltip("Effect Iterations")]
		public IntParameter Iterations = new IntParameter {value = 4};
		[Range(0f, 1f), Tooltip("Effect Intensity")]
		public FloatParameter Intensity = new FloatParameter {value = 0.5f};
		internal RenderTexture loopRT0;
		internal RenderTexture loopRT1;
	}

	public sealed class BlurRenderer : PostProcessEffectRenderer<Blur>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Blur"));
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