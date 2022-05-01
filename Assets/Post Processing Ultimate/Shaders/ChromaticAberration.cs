using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(ChromaticAberrationRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/ChromaticAberration")]
	public sealed class ChromaticAberration : PostProcessEffectSettings
	{
		[Range(1, 8), Tooltip("Effect iterations")]
		public IntParameter Iterations = new IntParameter {value = 1};
		[Range(0f, 1f), Tooltip("Channels offset")]
		public FloatParameter Offset = new FloatParameter {value = 0.5f};
		internal RenderTexture loopRT0;
		internal RenderTexture loopRT1;
	}

	public sealed class ChromaticAberrationRenderer : PostProcessEffectRenderer<ChromaticAberration>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/ChromaticAberration"));
			RenderTexture.ReleaseTemporary(settings.loopRT0);
			sheet.properties.SetFloat("_Iterations", settings.Iterations);
			sheet.properties.SetFloat("_Offset", settings.Offset);
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